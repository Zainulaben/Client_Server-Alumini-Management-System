using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Client.Models
{
    public class Student
    {
        public string? Number { get; set; }
        public string? Character { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Session { get; set; }
        public string? RollNumber { get; set; }
        public Brush? BgColor { get; set; }

        public bool CheckEmpty()
        {
            if (UserName == "" || Email == "" || Session == "" || RollNumber == "")
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

        public bool CheckSession()
        {
            if(Session == "Session")
            {
                return true;
            }
            return false;
        }
    }
}
