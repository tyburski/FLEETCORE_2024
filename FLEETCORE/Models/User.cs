using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using FLEETCORE.Models.Body;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FLEETCORE.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Password { get; private set; }
        public string EmailAddress { get; private set; }
        public int Role { get; private set; }

        public string Create(SignUpBody body)
        {
            if (body.FirstName.Length > 1 && body.FirstName.Length < 60 && !body.FirstName.Any(char.IsDigit))
            {
                if (body.LastName.Length > 1 && body.LastName.Length < 60 && !body.LastName.Any(char.IsDigit))
                {
                    var email = new EmailAddressAttribute();
                    if (email.IsValid(body.EmailAddress))
                    {
                        if (body.Password.Length > 6 
                            && body.Password.Any(Char.IsUpper) 
                            && body.Password.Any(c => !char.IsLetterOrDigit(c))
                            && body.Password.Any(char.IsDigit)) {

                            FirstName = $"{body.FirstName[0].ToString().ToUpper()}{body.FirstName.Substring(1)}";
                            LastName = $"{body.LastName[0].ToString().ToUpper()}{body.LastName.Substring(1)}";
                            EmailAddress = body.EmailAddress;
                            Role = 0;

                            Password = BCrypt.Net.BCrypt.HashPassword(body.Password);

                            return "done";
                        }
                        else return "passwordFormat";                          
                    }
                    else return "emailAddressFormat";
                }
                else return "lastNameFormat";
            }
            else return "firstNameFormat";
        }

        public int SwitchRole()
        {
            if(Role.Equals(0)) Role = 1;
            else Role = 0;

            return Role;
        }
    }
}
