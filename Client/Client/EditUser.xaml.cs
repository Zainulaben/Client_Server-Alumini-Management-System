using Client.Models;
using Client.Pages;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client
{
    public partial class EditUser : Window
    {
        DashboardPage dashboardPage;
        CurrentUser currentUser;
        int userdindex;
        Frame frame;
        TcpClient client;

        public EditUser(CurrentUser currentUser, int index, Frame frame, TcpClient c)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            userdindex = index;
            this.frame = frame;
            client = c;

            FillData();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        //UserName Field
        private void textUsername_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUsername.Focus();
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

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            textUsername.Visibility = Visibility.Collapsed;
        }

        //Email Field
        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == null || txtEmail.Text == "")
            {
                textEmail.Visibility = Visibility.Visible;
            }
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            textEmail.Visibility = Visibility.Collapsed;
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            textEmail.Visibility = Visibility.Collapsed;
        }

        //Password Field
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

        //Phone Number Field
        private void textPnumber_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPnumber.Focus();
        }

        private void txtPnumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPnumber.Text == "" || txtPnumber.Text == null)
            {
                textPnumber.Visibility = Visibility.Visible;
            }
        }

        private void txtPnumber_GotFocus(object sender, RoutedEventArgs e)
        {
            textPnumber.Visibility = Visibility.Collapsed;
        }

        private void txtPnumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            textPnumber.Visibility = Visibility.Collapsed;
        }

        private void txtPnumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        //Edit Button
        private void editUserButton_Click(object sender, RoutedEventArgs e)
        {
            User user = new User
            {
                UserName = txtUsername.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Password,
                PhoneNumber = txtPnumber.Text,
                Role = cmbRole.SelectionBoxItem.ToString(),
            };

            if (user.CheckEmpty())
            {
                MessageBox.Show("Fill All the Fields", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (user.CheckUsername())
            {
                MessageBox.Show("Username not contain spaces", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (user.CheckEmailFormat())
            {
                txtEmail.Text = "";
                textEmail.Visibility = Visibility.Visible;
                MessageBox.Show("Invalid email format", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (user.CheckRole())
            {
                cmbRole.SelectedIndex = 0;
                MessageBox.Show("Don't Select Role in ComboBox", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (currentUser.Users.Where(u => u.Number != currentUser.Users[userdindex].Number).Any(u => u.UserName == user.UserName))
            {
                txtUsername.Text = "";
                textUsername.Visibility = Visibility.Visible;
                MessageBox.Show("UserName already exists", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (currentUser.Users.Where(u => u.Number != currentUser.Users[userdindex].Number).Any(u => u.Email == user.Email))
            {
                txtEmail.Text = "";
                textEmail.Visibility = Visibility.Visible;
                MessageBox.Show("Email already exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                user.Number = currentUser.Users[userdindex].Number;

                string message = "9," + user.Number + "," + user.UserName + "," + user.Password + "," + user.Email + ","
                    + user.PhoneNumber + "," + user.Role;

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
                    currentUser.Users[userdindex].UserName = user.UserName;
                    currentUser.Users[userdindex].Email = user.Email;
                    currentUser.Users[userdindex].Password = user.Password;
                    currentUser.Users[userdindex].PhoneNumber = user.PhoneNumber;
                    currentUser.Users[userdindex].Role = user.Role;

                    this.Close();
                    dashboardPage = new DashboardPage(currentUser, frame, false, client);
                    frame.Content = dashboardPage;
                    MessageBox.Show("User Data Edit Sucessfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("User Data Not Edited", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                }


            }
        }

        //Cancel Button
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Fill Data in Fields
        private void FillData()
        {
            textUsername.Visibility = Visibility.Collapsed;
            textEmail.Visibility = Visibility.Collapsed;
            textPassword.Visibility = Visibility.Collapsed;
            textPnumber.Visibility = Visibility.Collapsed;

            txtUsername.Text = currentUser.Users[userdindex].UserName;
            txtEmail.Text = currentUser.Users[userdindex].Email;
            txtPassword.Password = currentUser.Users[userdindex].Password;
            txtPnumber.Text = currentUser.Users[userdindex].PhoneNumber;
            cmbRole.Text = currentUser.Users[userdindex].Role;
        }
    }
}
