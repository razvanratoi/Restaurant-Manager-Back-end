using BookMySeatApi.Services;
using RestaurantManager.DTOs;
using RestaurantManager.Models;
using RestaurantManager.Repositories.Interfaces;

namespace RestaurantManager.Services
{
    public class UserService
    {
        private readonly IUserRepo _userRepo;
        private readonly JwtTokenService _tokenService;
        private readonly PasswordService _passwordService;
        public UserService(IUserRepo userRepo, PasswordService passwordService, JwtTokenService tokenService)
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public string Login(Credentials creds)
        {
            var user = _userRepo.FindByUsername(creds.Username).Result;

            if (user == null || !_passwordService.CheckPassword(creds.Password, user.Salt, user.Password))
            {
                return "Invalid username or password";
            }

            var token = _tokenService.CreateToken(user);

            return token;
        }

        public List<User> GetAll()
        {
            return _userRepo.GetAll().Result.ToList();
        }

        public User? GetById(int id)
        {
            return _userRepo.GetByIdAsync(id).Result;
        }

        public User Create(User user)
        {
            user.Salt = _passwordService.GenerateSalt();
            user.Password = _passwordService.HashPassword(user.Password, user.Salt);
            _userRepo.Add(user);
            return user;
        }

        public bool Delete(int id)
        {
            var user = _userRepo.GetByIdAsync(id).Result;
            if (user == null) return false;
            return _userRepo.Delete(user).Result;
        }

        public bool Update(User userParam)
        {
            var user = _userRepo.GetByIdAsync(userParam.Id).Result;

            if (user == null) return false;

            user.Username = userParam.Username;
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Email = userParam.Email;
            user.Role = userParam.Role;

            _userRepo.Update(user);
            return true;
        }

        public async Task<User> GetUserFromToken(HttpContext httpContext)
        {
            var userId = _tokenService.GetUserIdFromToken(httpContext);
            var userGot = await _userRepo.GetByIdAsync(userId);
            return userGot;
        }
        
    }
}