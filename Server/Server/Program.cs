using Server.Models;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Server
    {
        private static HashSet<string> connectedClients = new HashSet<string>(); // Store connected client IP addresses

        //-1 mean Client exit
        //1 mean check Username and Password
        //2 mean send all the user
        //3 mean send all the student
        //4 mean add new student
        //5 mean add new user
        //6 mean delete student
        //7 mean delete user
        //8 mean edit student
        //9 mean edit user

        static void Main()
        {
            DatabaseFunctions databaseFunctions = new DatabaseFunctions();
            TcpListener server = null;
            string ipAddress = "192.168.10.11";

            try
            {
                // Set the TcpListener on port 13000.
                int port = 13000;
                IPAddress localAddr = IPAddress.Parse(ipAddress);

                // TcpListener server will listen on this IP address and port.
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                Console.WriteLine("Server started. Waiting for connections...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    string clientAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

                    if (!connectedClients.Contains(clientAddress))
                    {
                        Console.WriteLine($"Client with address {clientAddress} connected.");
                        connectedClients.Add(clientAddress);
                        Task.Run(() => HandleClient(client, databaseFunctions, clientAddress));
                    }
                    else
                    {
                        Console.WriteLine($"Client with address {clientAddress} tried to reconnect too soon. Ignoring.");
                        client.Close();
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }

        static void HandleClient(TcpClient client, DatabaseFunctions databaseFunctions, string clientAddress)
        {
            string data = null;
            try
            {
                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();
                byte[] bytes = new byte[4096];

                // Loop to receive all the data sent by the client.
                while (true)
                {
                    int i = stream.Read(bytes, 0, bytes.Length);
                    if (i == 0)
                        break; // Client disconnected

                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    string[] parts = data.Split(',');
                    string message = "true,";

                    if (parts[0] == "-1")
                    {
                        Console.WriteLine($"Client Address: {clientAddress} exited");
                        break;
                    }
                    else if (parts[0] == "1")
                    {
                        Console.WriteLine($"Client Address: {clientAddress} request for Checking Username and Password..........");
                        string role = databaseFunctions.CheckUser(parts[1], parts[2]);

                        if (role == "-1")
                        {
                            message = "false,";
                        }
                        else
                        {
                            int id = databaseFunctions.GetIDbyUserName(parts[1], role);
                            message += role + "," + id + ",";
                        }
                    }
                    else if (parts[0] == "2")
                    {
                        Console.WriteLine($"Client Address: {clientAddress} request for all the users data..........");
                        ObservableCollection<User> users = databaseFunctions.GetUsersForSuperAdmin(Convert.ToInt32(parts[1]));
                        message = "";
                        foreach (User u in users)
                        {
                            message += u.Number + ",";
                            message += u.UserName + ",";
                            message += u.Role + ",";
                            message += u.PhoneNumber + ",";
                            message += u.Email + ",";
                            message += u.Password + ";";
                        }
                    }
                    else if (parts[0] == "3")
                    {
                        Console.WriteLine($"Client Address: {clientAddress} request for all the students data..........");

                        ObservableCollection<Student> students = databaseFunctions.GetStudents();
                        message = "";
                        foreach (Student s in students)
                        {
                            message += s.Number + ",";
                            message += s.UserName + ",";
                            message += s.Email + ",";
                            message += s.Session + ",";
                            message += s.RollNumber + ";";
                        }
                    }
                    else if (parts[0] == "4")
                    {
                        Console.WriteLine($"Client Address: {clientAddress} request for Add student data..........");

                        Student student = new Student
                        {
                            UserName = parts[1],
                            Email = parts[2],
                            Session = parts[3],
                            RollNumber = parts[4]
                        };

                        bool flag = databaseFunctions.StoreStudent(student);

                        if (flag)
                        {
                            message = "true,";
                            int id = databaseFunctions.GetID_Stduent(student.UserName);

                            if (id != -1)
                            {
                                message += id + ",";
                            }
                        }
                        else
                        {
                            message = "false,";
                        }
                    }
                    else if (parts[0] == "5")
                    {
                        Console.WriteLine($"Client Address: {clientAddress} request for Add user data..........");

                        User user = new User
                        {
                            UserName = parts[1],
                            Password = parts[2],
                            Email = parts[3],
                            PhoneNumber = parts[4],
                            Role = parts[5]
                        };

                        bool flag = databaseFunctions.StoreUser(user);

                        if (flag)
                        {
                            message = "true,";
                            int id = databaseFunctions.GetIDbyUserName(user.UserName, user.Role);

                            if (id != -1)
                            {
                                message += id + ",";
                            }
                        }
                        else
                        {
                            message = "false,";
                        }
                    }
                    else if (parts[0] == "6")
                    {
                        Console.WriteLine($"Client Address: {clientAddress} request for Deleting student data..........");
                        int id = Convert.ToInt32(parts[1]);
                        bool flag = databaseFunctions.DeleteStudent(id);

                        if (flag)
                        {
                            message = "true,";
                        }
                        else
                        {
                            message = "false,";
                        }
                    }
                    else if (parts[0] == "7")
                    {
                        Console.WriteLine($"Client Address: {clientAddress} request for Deleting user data..........");
                        int id = Convert.ToInt32(parts[1]);
                        bool flag = databaseFunctions.DeleteUser(id);

                        if (flag)
                        {
                            message = "true,";
                        }
                        else
                        {
                            message = "false,";
                        }
                    }
                    else if (parts[0] == "8")
                    {
                        Console.WriteLine($"Client Address: {clientAddress} request for Editing student data..........");

                        Student student = new Student
                        {
                            Number = parts[1],
                            UserName = parts[2],
                            Email = parts[3],
                            Session = parts[4],
                            RollNumber = parts[5],
                        };

                        bool flag = databaseFunctions.UpdateStudent(student);
                        if (flag)
                        {
                            message = "true,";
                        }
                        else
                        {
                            message = "false,";
                        }

                    }
                    else if (parts[0] == "9")
                    {
                        Console.WriteLine($"Client Address: {clientAddress} request for Editing user data..........");

                        User user = new User
                        {
                            Number = parts[1],
                            UserName = parts[2],
                            Password = parts[3],
                            Email = parts[4],
                            PhoneNumber = parts[5],
                            Role = parts[6]
                        };

                        bool flag = databaseFunctions.UpdateUser(user);

                        if (flag)
                        {
                            message = "true,";
                        }
                        else
                        {
                            message = "false,";
                        }
                    }

                    byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                    stream.Write(messageBytes, 0, messageBytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                connectedClients.Remove(clientAddress);
                client.Close();
            }
        }
    }
}