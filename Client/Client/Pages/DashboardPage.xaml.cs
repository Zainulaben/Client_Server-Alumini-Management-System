using Client.Models;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.Pages
{
    public partial class DashboardPage : Page
    {

        private ICollectionView _view;
        private bool IsStudent;
        private CurrentUser CurrentUser;
        Frame frame;
        TcpClient client;

        public DashboardPage(CurrentUser currentUser, Frame frame, bool Student, TcpClient c)
        {
            InitializeComponent();

            membersDataGrid.ItemsSource = currentUser.Users;
            this.CurrentUser = currentUser;
            this.frame = frame;
            IsStudent = Student;
            client = c;

            if (IsStudent)
            {
                ForStudent();
            }

            CheckRole();
        }

        //Student Button
        private void StudentsButton_Click(object sender, RoutedEventArgs e)
        {
            ForStudent();
        }

        //User Button
        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            StudentsButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            UsersButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#784ff2"));

            FilterBlock.Text = "Search in User ...";
            title.Text = "Users";
            IsStudent = false;
            membersDataGrid.ItemsSource = CurrentUser.Users;

            PhoneNumber.Visibility = Visibility.Visible;
            Role.Visibility = Visibility.Visible;
            RollNumber.Visibility = Visibility.Hidden;
            Session.Visibility = Visibility.Hidden;
        }

        //Filter By Username
        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsStudent)
            {
                _view = CollectionViewSource.GetDefaultView(CurrentUser.Students);
            }
            else
            {
                _view = CollectionViewSource.GetDefaultView(CurrentUser.Users);
            }

            if (!string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                _view.Filter = item =>
                {
                    if (item is Student student)
                    {
                        return student.UserName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                    else if (item is Models.User user)
                    {
                        return user.UserName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                    return false;
                };
            }
            else
            {
                _view.Filter = null;
            }
        }

        private void ForStudent()
        {
            StudentsButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#784ff2"));
            UsersButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));

            FilterBlock.Text = "Search in Student ...";
            title.Text = "Students";
            IsStudent = true;
            membersDataGrid.ItemsSource = CurrentUser.Students;

            PhoneNumber.Visibility = Visibility.Hidden;
            Role.Visibility = Visibility.Hidden;
            RollNumber.Visibility = Visibility.Visible;
            Session.Visibility = Visibility.Visible;
        }

        //Edit Button
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            int index = membersDataGrid.SelectedIndex;

            if (IsStudent)
            {
                EditStudent editStudent = new EditStudent(CurrentUser, index, frame,client);
                editStudent.Show();
            }
            else
            {
                EditUser editUser = new EditUser(CurrentUser, index, frame,client);
                editUser.Show();
            }
        }

        //Delete Button
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            int index = membersDataGrid.SelectedIndex;

            if (IsStudent)
            {
                string message = "6," + CurrentUser.Students[index].Number;

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
                    CurrentUser.Students.RemoveAt(index);
                    membersDataGrid.ItemsSource = CurrentUser.Students;
                    MessageBox.Show("Student Delete Sucessfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {                
                    MessageBox.Show("Student Not Deleted", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                string message = "7," + CurrentUser.Users[index].Number;

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
                    CurrentUser.Users.RemoveAt(index);
                    membersDataGrid.ItemsSource = CurrentUser.Students;
                    MessageBox.Show("User Delete Sucessfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("User Not Deleted", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // According to Role Set GUI
        private void CheckRole()
        {
            if (CurrentUser.role == "Clerk")
            {
                UsersButton.Visibility = Visibility.Collapsed;
                Operations.Visibility = Visibility.Hidden;
                ForStudent();
            }
            else if (CurrentUser.role == "Admin")
            {
                IsStudent = true;
                UsersButton.Visibility = Visibility.Collapsed;
                ForStudent();
            }
        }
    }
}
