using System.Text.RegularExpressions;
using Crypt = BCrypt.Net.BCrypt;

namespace BookMySeatApi.Services
{
    public class PasswordService 
    {
        public PasswordService()
        {
        }

        class Requirement {
            public string Restriction { get; }
            public string Output { get; }
            public Requirement(string Restriction, string Output)
            {
                this.Output = Output;
                this.Restriction = Restriction;
            }
        }

        public bool CheckComplexity(string Password)
        {
            List<Requirement> Requirements = new List<Requirement>();

            Requirements.Add(new Requirement("(?=.{8,})", "The password must contain at least 8 characters."));
            Requirements.Add(new Requirement("(?=.*[a-z])", "The password must contain a lower case letters."));
            Requirements.Add(new Requirement("(?=.*[A-Z])", "The password must contain a upper case letters."));
            Requirements.Add(new Requirement("(?=.*[0-9])", "The password must contain digits."));
            Requirements.Add(new Requirement("(?=.*[^A-Za-z0-9])", "The password must contain special characters."));

            foreach(Requirement r in Requirements){
                Regex Pattern = new Regex(r.Restriction);
                if(!Pattern.IsMatch(Password)){
                    throw new Exception(r.Output);
                }
            }
            
            return true;
        }

        public string GenerateSalt()
        {
            return Crypt.GenerateSalt(10);
        }

        public string HashPassword(string Password, string Salt){
            return Crypt.HashPassword(Password, Salt);
        }

        public bool CheckPassword(string Password, string Salt, string DbHashedPassword)
        {
            return Crypt.HashPassword(Password, Salt) == DbHashedPassword;
        }
    }
}