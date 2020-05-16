using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static IPAddress groupAddress = IPAddress.Parse("224.5.5.5");
        static UdpClient udpClientMenu = new UdpClient();
        static int receiveUserPort = 8999;
        static int sendPort = 8998;
        static Thread receiverThread;
        static TelegramModel model = new TelegramModel();
        static public bool changed = true;

        static void Main(string[] args)
        {
            udpClientMenu.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClientMenu.Client.Bind(new IPEndPoint(IPAddress.Any, receiveUserPort));
            udpClientMenu.JoinMulticastGroup(groupAddress);
            receiverThread = new Thread(Receiver);
            receiverThread.Start();
        }

        static public void Receiver()
        {
            while (true)
            {

                byte[] d;
                Users user1 = new Users();
                IPEndPoint sender = null;
                byte[] data = udpClientMenu.Receive(ref sender);
                string msg = Encoding.Default.GetString(data);
                Console.WriteLine(msg);
                if (msg[0] == '@')
                {
                    bool n = false;
                    bool p = false;


                    msg = msg.Replace("@", "");
                    string name = msg.Split(' ')[0];
                    string password = msg.Split(' ')[1];

                    foreach (Users users in model.Users)
                    {
                        if (users.Name == name)
                        {
                            n = true;
                            break;
                        }
                    }
                    if (n)
                    {
                        foreach (Users users in model.Users)
                        {
                            if (users.Password == password)
                            {
                                p = true;
                                break;
                            }
                        }
                        if (p)
                        {
                            d = Encoding.Default.GetBytes("true");
                            udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                        }
                        else
                        {
                            d = Encoding.Default.GetBytes("password");
                            udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                        }
                    }
                    else
                    {
                        d = Encoding.Default.GetBytes("name");
                        udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                    }
                }
                else if (msg[0] == '$')
                {
                    msg = msg.Replace("$", "");
                    string name = msg.Split(' ')[0];
                    string password = msg.Split(' ')[1];

                    foreach (Users user in model.Users)
                    {
                        if (user.Name == name)
                        {
                            d = Encoding.Default.GetBytes("name");
                            udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                            changed = false;
                            break;
                        }
                        else
                        {
                            changed = true;

                        }
                    }
                    if (changed)
                    {
                        user1 = new Users();
                        user1.Name = name;
                        user1.Password = password;

                        model.Users.Add(user1);

                        d = Encoding.Default.GetBytes("true");
                        udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));

                        model.SaveChanges();
                        changed = false;
                    }

                }
                else if (msg[0] == '%')
                {
                    string name = msg.Replace("%", "");
                    int k = 0;

                    foreach (Groups item in model.Groups)
                    {
                        foreach (Users user in item.Users)
                        {
                            if (user.Name == name)
                            {
                                k++;
                            }
                        }
                    }

                    d = Encoding.Default.GetBytes(k.ToString());
                    udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                    foreach (Groups item in model.Groups)
                    {
                        foreach (Users user in item.Users)
                        {
                            if (user.Name == name)
                            {
                                d = Encoding.Default.GetBytes(item.Name);
                                udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                            }
                        }
                    }


                }
                else if (msg[0] == '*')
                {
                    string name = msg.Replace("*", "");
                    int k = 0;
                    bool ks = true;

                    foreach (Groups item in model.Groups)
                    {
                        foreach (Users user in item.Users)
                        {
                            if (user.Name == name)
                            {
                                ks = false;
                                break;
                            }
                            else
                            {
                                ks = true;
                            }
                        }
                        if(item.Users.Count == 0)
                        {
                            ks = true;
                        }
                        if (ks)
                        {
                            k++;
                        }
                    }


                    d = Encoding.Default.GetBytes(k.ToString());
                    udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                    foreach (Groups item in model.Groups)
                    {
                        foreach (Users user in item.Users)
                        {
                            if (user.Name != name)
                            {
                                ks = true;
                            }
                            else
                            {
                                ks = false;
                                break;
                            }
                            
                        }
                        if (item.Users.Count == 0)
                        {
                            ks = true;
                        }
                        if (ks)
                        {
                            d = Encoding.Default.GetBytes(item.Name);
                            udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                        }

                    }

                }
                else if (msg[0] == '^')
                {
                    Users users = new Users();
                    Groups group = new Groups();
                    string name = msg.Replace("^", "").Split(' ')[0];
                    string groupName = msg.Replace("^", "").Split(' ')[1];
                    foreach (Users user in model.Users)
                    {
                        if (user.Name == name)
                        {
                            users = user;
                            break;
                        }
                    }
                    foreach (Groups item in model.Groups)
                    {
                        if (item.Name == groupName)
                        {
                            group = item;
                            break;
                        }
                    }
                    group.Users.Add(users);
                    users.Groups.Add(group);

                    model.SaveChanges();
                }
                else if (msg[0] == '(')
                {
                    Users users = new Users();
                    Groups group = new Groups();
                    string name = msg.Replace("(", "").Split(' ')[0];
                    string groupName = msg.Replace("(", "").Split(' ')[1];
                    foreach (Users user in model.Users)
                    {
                        if (user.Name == name)
                        {
                            users = user;
                            break;
                        }
                    }
                    group.Name = groupName;
                    group.Users.Add(users);
                    users.Groups.Add(group);

                    model.Groups.Add(group);

                    model.SaveChanges();
                }

            }
        }
    }
}
