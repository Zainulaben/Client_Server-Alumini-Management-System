using Client.Models;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client
{
    public partial class MainWindow : Window
    {
        int port = 13000;
        string IpAddress = "192.168.10.11";

        TcpClient client;
        CurrentUser currentUser = new CurrentUser();


        public MainWindow()
        {
            InitializeComponent();
            if (!MakeConnection())
            {
                Application.Current.Shutdown();
            }
        }

        //Exit Button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

            string message = "-1";
            byte[] data = Encoding.ASCII.GetBytes(message);
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            client.Close();
        }

        //Username Field
        private void textUsername_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUsername.Focus();
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            textUsername.Visibility = Visibility.Collapsed;
        }

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == null || txtUsername.Text == "")
            {
                textUsername.Visibility = Visibility.Visible;
            }
        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            textUsername.Visibility = Visibility.Collapsed;
        }

        //Password
        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            textPassword.Visibility = Visibility.Collapsed;
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password == null || txtPassword.Password == "")
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            textPassword.Visibility = Visibility.Collapsed;
        }

        //Sign In Button
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentUser.username = txtUsername.Text;
                currentUser.password = txtPassword.Password;

                if (!string.IsNullOrEmpty(currentUser.username) && !string.IsNullOrEmpty(currentUser.password))
                {
                    string message = "1,";
                    message += currentUser.username + ",";
                    message += currentUser.password;

                    // Send data for check user exist or not
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);

                    // String to store the response ASCII representation.
                    byte[] responseData = new byte[4096];

                    // Read the first batch of the TcpServer response bytes.
                    int bytesReceived = stream.Read(responseData, 0, responseData.Length);
                    string response = Encoding.ASCII.GetString(responseData, 0, bytesReceived);
                    string[] parts = response.Split(',');

                    if (parts[0] == "true")
                    {
                        currentUser.role = parts[1];
                        currentUser.id = Convert.ToInt32(parts[2]);

                        if (currentUser.role == "Super Admin")
                        {
                            LoadUser();
                        }
                        LoadStudent();

                        Dashboard dashboard = new Dashboard(currentUser,client);
                        this.Visibility = Visibility.Hidden;
                        dashboard.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Fill all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while communicating with the server: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //Frame Movement
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        //Connection Between Client and Server
        private bool MakeConnection()
        {
            try
            {
                client = new TcpClient(IpAddress, port);
                return true;
            }
            catch (ArgumentNullException e)
            {
               MessageBox.Show($"ArgumentNullException: {e.Message}","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
            catch (SocketException e)
            {
                MessageBox.Show($"SocketException: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        //Get All the Users from Database
        private void LoadUser()
        {
            try
            {
                string message = "2," + currentUser.id;
                byte[] data = Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                // Read response from server
                byte[] responseData = new byte[4096]; // Assuming a maximum message size
                int bytesReceived = stream.Read(responseData, 0, responseData.Length);
                string response = Encoding.ASCII.GetString(responseData, 0, bytesReceived);

                // Split the response by ';'
                string[] userData = response.Split(';');

                // Iterate through each user data and print
                foreach (string userString in userData)
                {
                    string[] userDetails = userString.Split(',');
                    if (userDetails.Length == 6) // Assuming each user should have exactly 6 fields
                    {
                        User user = new User
                        {
                            Number = userDetails[0],
                            UserName = userDetails[1],
                            Role = userDetails[2],
                            PhoneNumber = userDetails[3],
                            Email = userDetails[4],
                            Password = userDetails[5]
                        };

                        // Assuming User class has properties Character and BgColor
                        user.Character = user.UserName.Length > 0 ? user.UserName[0].ToString() : ""; // Ensure UserName is not empty
                        user.BgColor = Colors.CharColors(user.Character); // Assuming CharColors is a method returning SolidColorBrush

                        // Assuming currentUser is an instance of a class that contains a Users property of type List<User>
                        currentUser.Users.Add(user);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        //Get All the Students from Database
        private void LoadStudent()
        {
            try
            {
                string message = "3";
                byte[] data = Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                // Read response from server
                byte[] responseData = new byte[4096]; // Assuming a maximum message size
                int bytesReceived = stream.Read(responseData, 0, responseData.Length);
                string response = Encoding.ASCII.GetString(responseData, 0, bytesReceived);

                // Split the response by ';'
                string[] studentData = response.Split(';');

                // Iterate through each student data and print
                foreach (string studentString in studentData)
                {
                    string[] studentDetails = studentString.Split(',');
                    if (studentDetails.Length == 5) // Assuming each student should have exactly 5 fields
                    {
                        Student student = new Student
                        {
                            Number = studentDetails[0],
                            UserName = studentDetails[1],
                            Email = studentDetails[2],
                            Session = studentDetails[3],
                            RollNumber = studentDetails[4]
                        };

                        // Assuming Student class has properties Character and BgColor
                        student.Character = student.UserName.Length > 0 ? student.UserName[0].ToString() : ""; // Ensure UserName is not empty
                        student.BgColor = Colors.CharColors(student.Character); // Assuming CharColors is a method returning SolidColorBrush

                        // Assuming currentUser is an instance of a class that contains a Students property of type List<Student>
                        currentUser.Students.Add(student);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

    }
}
