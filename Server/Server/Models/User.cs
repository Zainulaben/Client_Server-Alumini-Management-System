using System.Drawing;
using System.Text.RegularExpressions;

namespace Server.Models
{
    public class User
    {
        public string? Number { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
