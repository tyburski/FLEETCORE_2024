using Azure;
using FLEETCORE.Models;
using FLEETCORE.Models.Body;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FLEETCORE.Services
{
    public interface IAccountService
    {
        string SignIn(SignInBody body);
        string SignUp(SignUpBody body);
        string SwitchRole(int id);
        string Delete(int id, string emailAddress);
    }
       
    public class AccountService : IAccountService
    {
        private readonly FLEETCOREDbContext context;
        private readonly IConfiguration config;
        public AccountService(FLEETCOREDbContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }
        public string SignIn(SignInBody body)
        {
            string result = "unauthorized";

            if (body.EmailAddress.Length > 0 && body.Password.Length > 0)
            {
                var user = context.Users
                    .Where(u => u.EmailAddress == body.EmailAddress)
                    .FirstOrDefault();

                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(body.Password, user.Password))
                    {
                        var tokenString = GenerateJSONWebToken(body, user.Role);
                        result = tokenString;
                    }
                }
                return result;
            }
            else return result;          
        }
        private string GenerateJSONWebToken(SignInBody body, int role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("User", body.EmailAddress),
                new Claim("Role", role.ToString())};

        var token = new JwtSecurityToken(config["Jwt:Issuer"],
              config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string SignUp(SignUpBody body)
        {
            var us = context.Users.Where(u => u.EmailAddress == body.EmailAddress).FirstOrDefault();

            if(us is null) 
            {
                var user = new User();

                var result = user.Create(body);
                if (result.Equals("done"))
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    return user.EmailAddress;
                }
                else return result;
            }            
            else return "user_exist";
        }

        public string SwitchRole(int id)
        {
            var user = context.Users
                    .Where(u => u.Id == id)
                    .FirstOrDefault();
            if (user != null)
            {
                var result = user.SwitchRole();
                context.SaveChanges();
                return result.ToString();
            }
            else return "wrong_user";
        }

        public string Delete(int id, string emailAddress)
        {
            var user = context.Users
                    .Where(u => u.Id == id)
                    .FirstOrDefault();

            if (user != null &&
                !emailAddress.Equals(user.EmailAddress))
            {
                context.Users.Remove(user);
                context.SaveChanges();
                return "done";
            }
            else return "wrong_user";
        }
    }
}
