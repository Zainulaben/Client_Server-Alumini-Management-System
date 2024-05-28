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
    public partial class EditStudent : Window
    {

        DashboardPage dashboardPage;
        CurrentUser currentUser;
        int stdindex;
        Frame frame;
        TcpClient client;


        public EditStudent(CurrentUser currentUser, int index, Frame frame, TcpClient c)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            stdindex = index;
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

        //Edit Button
        private void editStudentButton_Click(object sender, RoutedEventArgs e)
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
                txtEmail.Text = "";
                textEmail.Visibility = Visibility.Visible;
                MessageBox.Show("Invalid email format", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (student.CheckSession())
            {
                cmbSession.SelectedIndex = 0;
                MessageBox.Show("Don't Select Role in ComboBox", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (currentUser.Students.Where(s => s.Number != currentUser.Students[stdindex].Number).Any(s => s.UserName == student.UserName))
            {
                txtUsername.Text = "";
                textUsername.Visibility = Visibility.Visible;
                MessageBox.Show("UserName already exists", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (currentUser.Students.Where(s => s.Number != currentUser.Students[stdindex].Number).Any(s => s.Email == student.Email))
            {
                txtEmail.Text = "";
                textEmail.Visibility = Visibility.Visible;
                MessageBox.Show("Email already exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {

                student.Number = currentUser.Students[stdindex].Number;

                string message = "8," + student.Number + "," + student.UserName + "," + student.Email + "," + student.Session + "," + student.RollNumber;

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
                    currentUser.Students[stdindex].UserName = student.UserName;
                    currentUser.Students[stdindex].Email = student.Email;
                    currentUser.Students[stdindex].Session = student.Session;
                    currentUser.Students[stdindex].RollNumber = student.RollNumber;

                    this.Close();
                    dashboardPage = new DashboardPage(currentUser, frame, true, client);
                    frame.Content = dashboardPage;
                    MessageBox.Show("Student Data Edit Sucessfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Student Data Not Edited", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        //Cancel Button
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void txtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
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
        //RollNo Field
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

        private void FillData()
        {
            textUsername.Visibility = Visibility.Collapsed;
            textEmail.Visibility = Visibility.Collapsed;
            textRollNo.Visibility = Visibility.Collapsed;

            txtUsername.Text = currentUser.Students[stdindex].UserName;
            txtEmail.Text = currentUser.Students[stdindex].Email;
            cmbSession.Text = currentUser.Students[stdindex].Session;
            txtRollNo.Text = currentUser.Students[stdindex].RollNumber;
        }
    }
}
