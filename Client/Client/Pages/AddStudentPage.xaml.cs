using Client.Models;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Pages
{
    public partial class AddStudentPage : Page
    {
        CurrentUser current;
        TcpClient client;

        public AddStudentPage(CurrentUser currentUser, TcpClient c)
        {
            InitializeComponent();
            current = currentUser;
            client = c;
        }

        //Username Field
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
       
        //Roll Number Field
        private void textRollNo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtRollNo.Focus();
        }

        private void txtRollNo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtRollNo.Text == null || txtRollNo.Text == "")
            {
                textRollNo.Visibility = Visibility.Visible;
            }
        }

        private void txtRollNo_GotFocus(object sender, RoutedEventArgs e)
        {
            textRollNo.Visibility = Visibility.Collapsed;
        }

        private void txtRollNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            textRollNo.Visibility = Visibility.Collapsed;
        }

        //Add Student Button
        private void addStudentButton_Click(object sender, RoutedEventArgs e)
        {
            Student student = new Student
            {
                UserName = txtUsername.Text,
                Email = txtEmail.Text,
                Session = cmbSession.SelectionBoxItem.ToString(),
                RollNumber = txtRollNo.Text,
            };

            if (student.CheckEmpty())
            {
                MessageBox.Show("Fill all the fields", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (student.CheckEmailFormat())
            {
                textEmail.Visibility = Visibility.Visible;
                MessageBox.Show("Invalid email format", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (student.CheckSession())
            {
                cmbSession.SelectedIndex = 0;
                MessageBox.Show("Don't Select Role in ComboBox", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (current.Students.Any(s => s.UserName == student.UserName))
            {
                textUsername.Visibility = Visibility.Visible;
                MessageBox.Show("UserName already exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (current.Students.Any(s => s.Email == student.Email))
            {
                textEmail.Visibility = Visibility.Visible;
                MessageBox.Show("Email already exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string message = "4," + student.UserName + "," + student.Email + "," + student.Session + "," + student.RollNumber;

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
                    student.Number = parts[1];
                    student.Character = student.UserName[0].ToString();
                    student.BgColor = Colors.CharColors(student.Character);

                    current.Students.Add(student);
                    MakeFieldsEmpty();
                    MessageBox.Show("Student Sucessfully Added", "Infomation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Student Not Added", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Empty The Fields
        private void MakeFieldsEmpty()
        {
            txtUsername.Text = "";
            txtEmail.Text = "";
            cmbSession.SelectedIndex = 0;
            txtRollNo.Text = "";

            textUsername.Visibility = Visibility.Visible;
            textEmail.Visibility = Visibility.Visible;
            textRollNo.Visibility = Visibility.Visible;
        }
    }
}
