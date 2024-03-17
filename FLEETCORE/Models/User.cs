using System;

namespace FLEETCORE.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string PhoneNumber { get; private set; }
        public string EmailAddress { get; private set; }
        public string Role { get; private set; }
        public List<Refueling> Refuelings { get; set; }

        public List<TimeSheet> TimeSheets { get; set; }

        public void Create(string firstName, string lastName, string phoneNumber, string emailAddress, string role)
        {
            if (firstName.Length > 0 &&
                lastName.Length > 0 &&
                phoneNumber.Length > 0 &&
                emailAddress.Length > 0 &&
                role.Equals("Kierowca") || role.Equals("Biuro"))
            {
                FirstName = $"{firstName[0].ToString().ToUpper()}{firstName.Substring(1)}";
                LastName = $"{lastName[0].ToString().ToUpper()}{lastName.Substring(1)}";
                PhoneNumber = phoneNumber;
                EmailAddress = emailAddress;
                Role = role;

                Username = GenerateUsername(lastName);
                Password = BCrypt.Net.BCrypt.HashPassword(Username.Substring(0, 4));
            }
        }
        public string GenerateUsername(string lastname)
        {
            using (var db = new FLEETCOREDbContext())
            {
                bool done = false;
                var _username = String.Empty;
                while (done == false)
                {
                    var username = $"{new Random().Next(0, 9999).ToString("D4")}{lastname.ToLower()}";

                    var user = db.Users
                                    .Where(u => u.Username == username)
                                    .FirstOrDefault<User>();
                    if (user == null)
                    {
                        done = true;
                        _username = username;                       
                    }
                }
                return _username;
            }
        }
        public void Update(string firstName, string lastName, string phoneNumber, string emailAddress, string role)
        {
            if (!FirstName.Equals(firstName) && firstName.Length > 0) FirstName = firstName;
            if (!LastName.Equals(lastName) && lastName.Length > 0) LastName = lastName;
            if (!PhoneNumber.Equals(phoneNumber) && phoneNumber.Length > 0) PhoneNumber = phoneNumber;
            if (!EmailAddress.Equals(emailAddress) && emailAddress.Length > 0) EmailAddress = emailAddress;
            if (!Role.Equals(role) && (role.Equals("Kierowca") || role.Equals("Biuro"))) Role = role;
        }
    }
}
