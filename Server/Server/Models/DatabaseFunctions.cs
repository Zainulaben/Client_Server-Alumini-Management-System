using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Server.Models
{
    internal class DatabaseFunctions
    {
        private string con = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public string CheckUser(string username, string password)
        {
            try
            {
                string query = "SELECT * FROM [User] WHERE UserName = @uname and Password = @pas";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@uname", username);
                        cmd.Parameters.AddWithValue("@pas", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (reader.GetString(1) == username)
                                    {
                                        string role = reader.GetString(5);
                                        return role;
                                    }
                                }

                                return "-1";
                            }
                            else
                            {
                                return "-1";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while checking user: " + ex.Message);
                return "-1";
            }
        }

        public int GetIDbyUserName(string username, string role)
        {
            try
            {
                string query = "SELECT Id FROM [User] WHERE UserName = @uname and Role = @role";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@uname", username);
                        cmd.Parameters.AddWithValue("@role", role);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32(0);
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while checking user id: " + ex.Message);
                return -1;
            }
        }

        public int GetID_Stduent(string username)
        {
            try
            {
                string query = "SELECT Id FROM [Student] WHERE UserName = @uname";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@uname", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32(0);
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while checking student id: " + ex.Message);
                return -1;
            }
        }

        public ObservableCollection<User> GetUsersForSuperAdmin(int id)
        {
            try
            {
                string query = "SELECT * FROM [User] WHERE Id != @id";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            var users = new ObservableCollection<User>();

                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    Number = reader["Id"].ToString(),
                                    UserName = reader["UserName"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString()
                                };

                                users.Add(user);
                            }

                            return users;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving users: " + ex.Message);
                return new ObservableCollection<User>();
            }
        }

        public ObservableCollection<Student> GetStudents()
        {
            try
            {
                string query = "SELECT * FROM [Student]";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            var students = new ObservableCollection<Student>();

                            while (reader.Read())
                            {
                                Student student = new Student
                                {
                                    Number = reader["Id"].ToString(),
                                    UserName = reader["UserName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Session = reader["Session"].ToString(),
                                    RollNumber = reader["RollNumber"].ToString()
                                };

                                students.Add(student);
                            }

                            return students;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving students: " + ex.Message);
                return new ObservableCollection<Student>();
            }
        }

        public bool StoreStudent(Student std)
        {
            try
            {
                string query = "INSERT INTO [Student] (UserName, Email, Session, RollNumber) VALUES (@userName, @email, @session, @roll)";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@userName", std.UserName);
                        cmd.Parameters.AddWithValue("@email", std.Email);
                        cmd.Parameters.AddWithValue("@session", std.Session);
                        cmd.Parameters.AddWithValue("@roll", std.RollNumber);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while storing student: " + ex.Message);
                return false;
            }
        }

        public bool StoreUser(User user)
        {
            try
            {
                string query = "INSERT INTO [User] (UserName, Role, PhoneNumber, Email, Password) VALUES (@UserName, @Role, @PhoneNumber, @Email, @Password)";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserName", user.UserName);
                        cmd.Parameters.AddWithValue("@Role", user.Role);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while storing the user: " + ex.Message);
                return false;
            }
        }

        public bool DeleteUser(int id)
        {
            try
            {
                string query = "DELETE FROM [User] WHERE Id = @Id";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the user: " + ex.Message);
                return false;
            }
        }

        public bool DeleteStudent(int id)
        {
            try
            {
                string query = "DELETE FROM [Student] WHERE Id = @Id";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the student: " + ex.Message);
                return false;
            }
        }

        public bool UpdateStudent(Student student)
        {
            try
            {
                string query = "UPDATE [Student] SET UserName = @UserName, Email = @Email, Session = @Session, RollNumber = @RollNumber WHERE Id = @Number";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Number", student.Number);
                        cmd.Parameters.AddWithValue("@UserName", student.UserName);
                        cmd.Parameters.AddWithValue("@Email", student.Email);
                        cmd.Parameters.AddWithValue("@Session", student.Session);
                        cmd.Parameters.AddWithValue("@RollNumber", student.RollNumber);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the student: " + ex.Message);
                return false;
            }
        }

        public bool UpdateUser(User user)
        {
            try
            {
                string query = "UPDATE [User] SET UserName = @UserName, Role = @Role, PhoneNumber = @PhoneNumber, Email = @Email, Password = @Password WHERE Id = @Number";

                using (SqlConnection connection = new SqlConnection(con))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Number", user.Number);
                        cmd.Parameters.AddWithValue("@UserName", user.UserName);
                        cmd.Parameters.AddWithValue("@Role", user.Role);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the user: " + ex.Message);
                return false;
            }
        }
    }
}
