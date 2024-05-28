using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Client.Models;
using Client.Pages;

namespace Client
{
    public partial class Dashboard : Window
    {
        CurrentUser CUser;
        TcpClient client;


        private bool IsMaximized = false;
        private bool IsDashboard = false;
        private bool IsAddUser = false;
        private bool IsStudent = false;

        DashboardPage dashboardPage;
        AddUserPage userPage;
        AddStudentPage studentPage;

        public Dashboard(CurrentUser u, TcpClient c)
        {
            InitializeComponent();

            CUser = u;
            client = c;

            Name.Text = CUser.username;
            Role.Text = CUser.role;

            CheckUserRole();

            dashboardPage = new DashboardPage(CUser, myframe, false, client);
            myframe.Content = dashboardPage;
            IsDashboard = true;
        }

        //Dashboard Button
        private void dashboard_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDashboard)
            {
                SolidColorBrush transparentBrush = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                SolidColorBrush purpleBrush = new SolidColorBrush(Color.FromRgb(123, 92, 214));
                SolidColorBrush whiteBrush = Brushes.White;

                dashboard.Background = purpleBrush;
                dashboard.Foreground = whiteBrush;

                addstudent.Background = transparentBrush;
                addstudent.Foreground = new SolidColorBrush(Color.FromRgb(208, 192, 255));

                adduser.Background = transparentBrush;
                adduser.Foreground = new SolidColorBrush(Color.FromRgb(208, 192, 255));

                IsAddUser = false;
                IsDashboard = true;
                IsStudent = false;

                dashboardPage = new DashboardPage(CUser, myframe, false, client);
                myframe.Content = dashboardPage;
            }
        }

        //Add User Button
        private void adduser_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAddUser)
            {
                SolidColorBrush transparentBrush = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                SolidColorBrush purpleBrush = new SolidColorBrush(Color.FromRgb(123, 92, 214));
                SolidColorBrush whiteBrush = Brushes.White;

                dashboard.Background = transparentBrush;
                dashboard.Foreground = new SolidColorBrush(Color.FromRgb(208, 192, 255));
                addstudent.Background = transparentBrush;
                addstudent.Foreground = new SolidColorBrush(Color.FromRgb(208, 192, 255));

                adduser.Background = purpleBrush;
                adduser.Foreground = whiteBrush;

                IsAddUser = true;
                IsDashboard = false;
                IsStudent = false;

                userPage = new AddUserPage(CUser, client);
                myframe.Content = userPage;
            }
        }

        //Add Student Button
        private void addstudent_Click(object sender, RoutedEventArgs e)
        {
            if (!IsStudent)
            {
                SolidColorBrush transparentBrush = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
                SolidColorBrush purpleBrush = new SolidColorBrush(Color.FromRgb(123, 92, 214));
                SolidColorBrush whiteBrush = Brushes.White;

                dashboard.Background = transparentBrush;
                dashboard.Foreground = new SolidColorBrush(Color.FromRgb(208, 192, 255));

                addstudent.Background = purpleBrush;
                addstudent.Foreground = whiteBrush;

                adduser.Background = transparentBrush;
                adduser.Foreground = new SolidColorBrush(Color.FromRgb(208, 192, 255));

                IsAddUser = false;
                IsDashboard = false;
                IsStudent = true;

                studentPage = new AddStudentPage(CUser, client);
                myframe.Content = studentPage;
            }
        }

        //Logout Button
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            string message = "-1";
            byte[] data = Encoding.ASCII.GetBytes(message);
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            client.Close();

            MainWindow main = new MainWindow();
            this.Visibility = Visibility.Hidden;
            main.Show();
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

        //Frame Movement
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        //Frame Size
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 600;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }

            }
        }

        //Check User Role ('Super Admin', 'Admin', 'Clerk')
        private void CheckUserRole()
        {
            if (CUser.role == "Super Admin")
            {
                image.ImageSource = new BitmapImage(new Uri("D:\\All Projects\\EAD Project\\Client_Server\\Client\\Client\\Images\\superadmin.jpg"));
            }
            else if (CUser.role == "Admin")
            {
                adduser.Visibility = Visibility.Hidden;
                image.ImageSource = new BitmapImage(new Uri("D:\\All Projects\\EAD Project\\Client_Server\\Client\\Client\\Images\\admin.jpg"));
            }
            else if (CUser.role == "Clerk")
            {
                adduser.Visibility = Visibility.Hidden;
                addstudent.Visibility = Visibility.Hidden;
                image.ImageSource = new BitmapImage(new Uri("D:\\All Projects\\EAD Project\\Client_Server\\Client\\Client\\Images\\person.png"));
            }
        }

        //Minimized the Window
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                // Minimize each window
                window.WindowState = WindowState.Minimized;
            }
        }
    }
}
