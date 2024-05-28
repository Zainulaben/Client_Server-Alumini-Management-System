using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Client.Models
{
    public class User
    {
        public string? Number { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Brush? BgColor { get; set; }
        public string? Character { get; set; }

        public bool CheckEmpty()
        {
            if (UserName == "" || Email == "" || Role == "" || PhoneNumber == "" || Password == "")
            {
                return true;
            }

            return false;
        }

        public bool CheckUsername()
        {
            if (UserName.Contains(" "))
            {
                return true; 
            }
            return false;
        }


        public bool CheckEmailFormat()
        {
            // Regular expression pattern for email validation
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(Email, emailPattern))
            {
                return true;
            }

            return false;
        }

        public bool CheckRole()
        {
            if (Role == "Role")
            {
                return true;
            }

            return false;
        }
    }
}
