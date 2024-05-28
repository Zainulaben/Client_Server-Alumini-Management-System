using System.Collections.ObjectModel;

namespace Client.Models
{
    public class CurrentUser
    {
        public ObservableCollection<User> Users;
        public ObservableCollection<Student> Students;
        public int id;
        public string? username;
        public string? password;
        public string? role;
        public CurrentUser()
        {
            Users = new ObservableCollection<User>();
            Students = new ObservableCollection<Student>();
        }
    }
}
