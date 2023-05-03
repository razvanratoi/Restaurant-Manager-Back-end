using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RestaurantManager.Models;
using Microsoft.IdentityModel.Tokens;

namespace BookMySeatApi.Services
{
    public class JwtTokenService
    {
        private const int JwtTokenExpireTime = 60;
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(User entity)
        {
            var expiration = DateTime.UtcNow.AddMinutes(JwtTokenExpireTime);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                CreateClaims(entity),
                expires: expiration,
                signingCredentials: CreateSigningCredentials()
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public int GetUserIdFromToken(HttpContext context) {
            string token = context.Request.Headers["Authorization"]!;
            token = token.Replace("Bearer", "").Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

            return System.Convert.ToInt32(securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        }

        private List<Claim> CreateClaims(User user)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, "UserAuthentificationToken"),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(JwtTokenExpireTime).ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            return claims;
        }

        private SigningCredentials CreateSigningCredentials() => new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }
}