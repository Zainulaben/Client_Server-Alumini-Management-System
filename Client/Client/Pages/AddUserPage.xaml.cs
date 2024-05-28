using Client.Models;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Pages
{
    public partial class AddUserPage : Page
    {
        CurrentUser currentUser;
        TcpClient client;

        public AddUserPage(CurrentUser currentUser, TcpClient c)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            client = c;
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

        //Add User Button
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
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
            else if (currentUser.Users.Any(u => u.UserName == user.UserName))
            {
                txtUsername.Text = "";
                textUsername.Visibility = Visibility.Visible;
                MessageBox.Show("UserName already exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (currentUser.Users.Any(u => u.Email == user.Email))
            {
                txtEmail.Text = "";
                textEmail.Visibility = Visibility.Visible;
                MessageBox.Show("Email already exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string message = "5," + user.UserName + "," + user.Password + "," + user.Email + "," + user.PhoneNumber + "," + user.Role;

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
                    user.Number = parts[1];
                    user.Character = user.UserName[0].ToString();
                    user.BgColor = Colors.CharColors(user.Character);

                    currentUser.Users.Add(user);
                    MakeFieldsEmpty();
                    MessageBox.Show("User Sucessfully Added", "Infomation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("User Not Added", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Make Fields Empty
        private void MakeFieldsEmpty()
        {
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtPnumber.Text = "";
            cmbRole.SelectedIndex = 0;

            textEmail.Visibility = Visibility.Visible;
            textUsername.Visibility = Visibility.Visible;
            textPassword.Visibility = Visibility.Visible;
            textPnumber.Visibility = Visibility.Visible;
        }
    }
}
