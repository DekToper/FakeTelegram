using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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
                if (msg[0] == '@') // Login 
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
                else if (msg[0] == '$') // Register
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

                    Directory.CreateDirectory("../../UserData/" + name);

                }
                else if (msg[0] == '%') // 
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
                else if (msg[0] == '*') // Get groups in witch user consist
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
                else if (msg[0] == '^') // Join to group
                {
                    Users users = new Users();
                    Groups group = new Groups();
                    string name = msg.Replace("^", "").Split(' ')[0];
                    string groupName = msg.Replace("^", "").Split(' ')[1];
                    users = GetUser(name);
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
                else if (msg[0] == '(') // Add new group
                {
                    Users users = new Users();
                    Groups group = new Groups();
                    string name = msg.Replace("(", "").Split(' ')[0];
                    string groupName = msg.Replace("(", "").Split(' ')[1];

                    users = GetUser(name);

                    group.Name = groupName;
                    group.Users.Add(users);
                    users.Groups.Add(group);

                    Directory.CreateDirectory($"../../GroupData/{groupName}");

                    StreamWriter streamWriter = new StreamWriter($"../../GroupData/{groupName}/chatLog.xml",true);

                    streamWriter.WriteLine("<?xml version=\"1.0\"?>");
                    streamWriter.WriteLine("<Messages>");
                    streamWriter.WriteLine("</Messages>");

                    streamWriter.Close();
                    streamWriter.Dispose();

                    model.Groups.Add(group);

                    model.SaveChanges();
                }
                else if(msg[0] == 'ё') // Set chat log
                {
                    string name = msg.Replace("ё", "").Split('Ё')[0];
                    string groupName = msg.Replace("ё", "").Split('Ё')[1];
                    string dateTime = msg.Replace("ё", "").Split('Ё')[2];
                    string message = msg.Replace("ё", "").Split('Ё')[3];
                    string path = "";
                    if (groupName.Contains("$"))
                    {
                        path = $"../../UserData/{name}/chatWith{groupName.Replace("$", "")}.xml";
                        SaveMessage(path, message, dateTime, name);
                        path = $"../../UserData/{groupName.Replace("$", "")}/chatWith{name}.xml";
                        SaveMessage(path, message, dateTime, name);
                    }
                    else
                    {
                        path = $"../../GroupData/{groupName}/chatLog.xml";
                        SaveMessage(path, message, dateTime, name);
                    }

                    
                }
                else if(msg[0] == '|') // Get chat log
                {
                    string name = msg.Replace("|", "").Split('/')[0];
                    string path = "";
                    string xmlFile = "";
                    if(msg.Replace("|", "").Split('/')[1] == "Group")
                    {
                        path = "GroupData";
                        xmlFile = "chatLog.xml";
                    }
                    else
                    {
                        xmlFile = "chatWith" + msg.Replace("|", "").Split('/')[2] + ".xml";
                        path = "UserData";
                    }
                    Messages messages = Serializer.Deserialize<Messages>($"../../{path}/{name}/{xmlFile}");
                    int k = messages.Message.Count;
                    byte[] count = Encoding.Default.GetBytes(k.ToString());
                    udpClientMenu.Send(count, count.Length, new IPEndPoint(groupAddress, sendPort));

                    for(int i = 0;  i < k; i++)
                    {
                        string text = messages.Message[i].Text;
                        string time = messages.Message[i].Time;
                        string fromUser = messages.Message[i].fromUser;

                        d = Encoding.Default.GetBytes(text + "&" + time + "&" + fromUser);

                        udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                    }
                }
                else if(msg[0] == '/') // Get all friends
                {
                    string name = msg.Replace("/", "");
                    Users users = GetUser(name);
                    int k = users.Users1.Count;
                    byte[] count = Encoding.Default.GetBytes(k.ToString());
                    udpClientMenu.Send(count, count.Length, new IPEndPoint(groupAddress, sendPort));

                    foreach (Users user in users.Users1)
                    {
                        d = Encoding.Default.GetBytes(user.Name);
                        udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                    }

                }
                else if(msg[0] == '\\')
                {
                    string name = msg.Replace("\\", "").Split('/')[0];
                    string friendName = msg.Replace("\\", "").Split('/')[1];

                    Users user = GetUser(name);
                    Users friendUser = GetUser(friendName);

                    user.Users1.Add(friendUser);
                    friendUser.Users2.Add(user);

                    model.SaveChangesAsync();

                    string path = "../../UserData/" + name + "/chatWith" + friendName + ".xml";
                   
                    StreamWriter streamWriter = new StreamWriter(path,true);

                    streamWriter.WriteLine("<?xml version=\"1.0\"?>");
                    streamWriter.WriteLine("<Messages>");
                    streamWriter.WriteLine("</Messages>");

                    streamWriter.Close();
                    streamWriter.Dispose();

                }
                else if(msg[0] == '=') // Get all contacts
                {
                    string name = msg.Replace("=", "");
                    Users users = GetUser(name);
                    int k = GetUnknownUserCount(name);
                    byte[] count = Encoding.Default.GetBytes(k.ToString());
                    udpClientMenu.Send(count, count.Length, new IPEndPoint(groupAddress, sendPort));
                    foreach (Users item in model.Users)
                    {
                        if(item.Name != name && !users.Users1.Contains(item))
                        {
                            d = Encoding.Default.GetBytes(item.Name);
                            udpClientMenu.Send(d, d.Length, new IPEndPoint(groupAddress, sendPort));
                        }
                    }

                    
                }

            }

        }

        static internal void SaveMessage(string path, string message, string dateTime, string name)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);

            XmlElement xRoot = xDoc.DocumentElement;

            XmlElement msgElem = xDoc.CreateElement("Message");

            XmlElement textElem = xDoc.CreateElement("Text");
            XmlText msgText = xDoc.CreateTextNode(message);
            textElem.AppendChild(msgText);
            XmlElement timeElem = xDoc.CreateElement("Time");
            XmlText timeText = xDoc.CreateTextNode(dateTime);
            timeElem.AppendChild(timeText);
            XmlElement fromUserElem = xDoc.CreateElement("fromUser");
            XmlText fromUserText = xDoc.CreateTextNode(name);
            fromUserElem.AppendChild(fromUserText);

            msgElem.AppendChild(textElem);
            msgElem.AppendChild(timeElem);
            msgElem.AppendChild(fromUserElem);

            xRoot.AppendChild(msgElem);
            xDoc.Save(path);
        }


        static internal int GetUnknownUserCount(string name)
        {
            int k = 0;
            Users user = GetUser(name);
            foreach (Users item in model.Users)
            {
                if (item.Name != name && !user.Users1.Contains(item))
                {
                    k++;
                }
                    
            }
            return k;
        }

        static internal Users GetUser(string name)
        {
            Users users = new Users();
            foreach (Users user in model.Users)
            {
                if (user.Name == name)
                {
                    users = user;
                    break;
                }
            }
            return users;
        }
    }
}
