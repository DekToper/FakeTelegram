using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using NAudio;
using System.Speech.Recognition;
using System.IO;
using FileHandlerLib;
namespace WpfApp11
{
    public class ChatHandler
    {
        Telegram telegram;

        public object Dispatcher { get; private set; }

        public ChatHandler(Telegram t)
        {
            telegram = t;
        }
        public void AddGroup(string groupName, bool groupUser)
        {
            Group group;
            Frame frame = new Frame();
            if (groupUser)
            {
                group = new Group("User");
                frame.Tag = "User";
                group.Date.Content = "User";
            }
            else
            {
                group = new Group("Group");
                frame.Tag = "Group";
                group.Date.Content = "Group";
            }
            group.GroupName.Content = groupName;
            group.Text.Content = "";
            frame.Navigate(group);
            frame.Name = group.GroupName.Content.ToString();
            telegram.listbox.Items.Add(frame);
        }

        public void SendMessageToServer(string userName, string groupName, string dataTime, string message)
        {
            byte[] data = Encoding.Default.GetBytes($"ё{userName}Ё{groupName}Ё{dataTime}Ё{message}");

            telegram.serverConect.udpClient.Send(data, data.Length, new IPEndPoint(telegram.serverConect.groupAddress, telegram.serverConect.sendPort));
        }


        public void Send(byte[] data, IPEndPoint iPEndPoint)
        {
            telegram.serverConect.udpClient.Send(data, data.Length, iPEndPoint);
        }

        public void LoadGroups()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(telegram.serverConect.groupAddress, telegram.serverConect.sendPort);
            byte[] data = Encoding.Default.GetBytes("%" + telegram.user.name);
            Send(data, iPEndPoint);
            IPEndPoint sender = null;
            byte[] kol = telegram.serverConect.udpClient.Receive(ref sender);
            int j = Convert.ToInt32(Encoding.Default.GetString(kol));

            for (int i = 0; i < j; i++)
            {
                byte[] groupData = telegram.serverConect.udpClient.Receive(ref sender);
                string groupInfo = Encoding.Default.GetString(groupData);
                AddGroup(groupInfo,false);

            }

        }

        internal void LoadFriends()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(telegram.serverConect.groupAddress, telegram.serverConect.sendPort);
            byte[] data = Encoding.Default.GetBytes("/" + telegram.user.name);
            Send(data, iPEndPoint);
            IPEndPoint sender = null;
            byte[] kol = telegram.serverConect.udpClient.Receive(ref sender);
            int j = Convert.ToInt32(Encoding.Default.GetString(kol));

            for (int i = 0; i < j; i++)
            {
                byte[] userData = telegram.serverConect.udpClient.Receive(ref sender);
                string userName = Encoding.Default.GetString(userData);
                AddGroup(userName,true);
            }
        }

        public void GetAllGroups()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(telegram.serverConect.groupAddress, telegram.serverConect.sendPort);
            byte[] data = Encoding.Default.GetBytes("*" + telegram.user.name);
            Send(data, iPEndPoint);
            IPEndPoint sender = null;
            byte[] kol = telegram.serverConect.udpClient.Receive(ref sender);
            try
            {
                int j = Convert.ToInt32(Encoding.Default.GetString(kol));
                for (int i = 0; i < j; i++)
                {
                    byte[] groupData = telegram.serverConect.udpClient.Receive(ref sender);
                    string groupInfo = Encoding.Default.GetString(groupData);
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Content = groupInfo;
                    listViewItem.HorizontalContentAlignment = HorizontalAlignment.Left;
                    listViewItem.HorizontalAlignment = HorizontalAlignment.Left;
                    telegram.groupList.listGroup.Items.Add(listViewItem);
                }
            }
            catch
            {

            }
        }

        internal void GetAllUsers()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(telegram.serverConect.groupAddress, telegram.serverConect.sendPort);
            byte[] data = Encoding.Default.GetBytes("=" + telegram.user.name);
            Send(data, iPEndPoint);
            IPEndPoint sender = null;
            byte[] kol = telegram.serverConect.udpClient.Receive(ref sender);
            int j = Convert.ToInt32(Encoding.Default.GetString(kol));
            for (int i = 0; i < j; i++)
            {
                byte[] userData = telegram.serverConect.udpClient.Receive(ref sender);
                string userInfo = Encoding.Default.GetString(userData);
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Content = userInfo;
                listViewItem.HorizontalContentAlignment = HorizontalAlignment.Left;
                listViewItem.HorizontalAlignment = HorizontalAlignment.Left;
                telegram.contactsPage.contactsList.Items.Add(listViewItem);

            }
        }

        public void HidePage(UIElement element)
        {
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(Hide);
            Thread thread = new Thread(threadStart);
            thread.Start(element);
        }

        private void Hide(object obj)
        {
            UIElement element = obj as UIElement;
            Thread.Sleep(200);
            telegram.Dispatcher.Invoke(new Action(() =>
            {
                element.Visibility = Visibility.Hidden;
            }
            ));
            Thread.CurrentThread.Abort();
        }

        public void ShowFromUser(string fromUser, string dateTime)
        {

            ListViewItem listViewItemUser = new ListViewItem();
            if (fromUser == telegram.user.name)
            {
                listViewItemUser.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else
            {
                listViewItemUser.HorizontalAlignment = HorizontalAlignment.Left;
            }

            Message msgPage = new Message();
            msgPage.timeMessage.Content = dateTime; 
            msgPage.fromUser.Content = fromUser;
            Frame frame = new Frame();
            frame.Navigate(msgPage);
            listViewItemUser.Content = frame;
            telegram.chatPage.Items.Add(listViewItemUser);
        }

        public void ShowMessage(string toGroup,string fromUser, string msg, string dateTime)
        {
            if (toGroup == telegram.curentGroup || fromUser == "Telegram")
            {
                ShowFromUser(fromUser,dateTime);
                ListViewItem listViewItemMessage = new ListViewItem();

                if (fromUser == telegram.user.name)
                {
                    listViewItemMessage.HorizontalAlignment = HorizontalAlignment.Right;
                }
                else
                {
                    listViewItemMessage.HorizontalAlignment = HorizontalAlignment.Left;
                }

                UserMessage userMessage = new UserMessage();
                userMessage.userMessage.Content = msg;
                Frame frame1 = new Frame();
                frame1.Navigate(userMessage);
                listViewItemMessage.Content = frame1;
                telegram.chatPage.Items.Add(listViewItemMessage);
            }

        }

        public void ShowFileMessage(string fromUser, UIElement element)
        {
            telegram.Dispatcher.Invoke(new Action(() =>
            {
                ShowFromUser(fromUser, DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                ListViewItem listViewItemMessage = new ListViewItem();

                if (fromUser == telegram.user.name)
                {
                    listViewItemMessage.HorizontalAlignment = HorizontalAlignment.Right;
                }
                else
                {
                    listViewItemMessage.HorizontalAlignment = HorizontalAlignment.Left;
                }
                listViewItemMessage.Content = element;
                telegram.chatPage.Items.Add(listViewItemMessage);
            }));
        }

        internal void getMessage()
        {
            while (true)
            {
                IPEndPoint sender = null;
                byte[] data = telegram.udpChatClient.Receive(ref sender);
                string mg = Encoding.Default.GetString(data);
                if (mg == "#")
                {
                    data = telegram.udpChatClient.Receive(ref sender);
                    mg = Encoding.Default.GetString(data);


                    string toGroup = mg.Split('/')[0];
                    string fromUser = mg.Split('/')[1];
                    string message = mg.Split('/')[2];

                    telegram.Dispatcher.Invoke(new Action(() =>
                    {
                        if(toGroup.Contains("%"))
                        {
                            string toUser = toGroup.Split('%')[1];
                            if(toUser == telegram.user.name)
                            {

                            }
                            else
                            {
                                if (fromUser == telegram.user.name)
                                {

                                }
                                else
                                {
                                    mg = null;
                                }
                            }
                        }
                        else if (toGroup == (telegram.listbox.SelectedItem as Frame).Name)
                        {
                            mg = fromUser + "/" + message;
                        }
                        else
                        {
                            mg = null;
                        }

                        if (mg != null)
                        {
                            ShowMessage(telegram.curentGroup, fromUser, message, DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                        }
                    }));

                }
                else if (mg == "@")
                {
                    string user = Encoding.Default.GetString(telegram.udpChatClient.Receive(ref sender));

                    int length = Convert.ToInt32(Encoding.Default.GetString(telegram.udpChatClient.Receive(ref sender)));
                    data = new byte[length];

                    for (int i = 0; i < FileHandler.GetFilePartsNumber(length) + 1; i++)
                    {
                        data = FileHandler.GetFileFromParts(data, telegram.udpChatClient.Receive(ref sender), i);
                    }

                    string fileType = Encoding.Default.GetString(telegram.udpChatClient.Receive(ref sender));

                    string filename = FileHandler.GetFileName(fileType);
                    try
                    {
                        File.WriteAllBytes(filename, data);
                    }
                    catch
                    { }

                    telegram.Dispatcher.Invoke(new Action(() =>
                    {
                        Frame frame = new Frame();
                        if (fileType == ".wav")
                        {
                            Wave wavePage = new Wave();
                            wavePage.musicName.Content = filename.Split('\\')[filename.Split('\\').Length - 1];
                            wavePage.playButton.Click += telegram.clickHandler.playButton_Click;
                            frame = new Frame();
                            frame.Navigate(wavePage);
                        }
                        else if (fileType == ".png" || fileType == ".jpg")
                        {
                            System.Drawing.Image img = System.Drawing.Image.FromFile(filename);
                            Bitmap bitmap = new Bitmap(img);
                            IntPtr imagePtr = bitmap.GetHbitmap();
                            ImageSource image = Imaging.CreateBitmapSourceFromHBitmap(
                            imagePtr, IntPtr.Zero, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                            ImagePage imagePage = new ImagePage();
                            imagePage.image.Source = image;
                            frame = new Frame();
                            frame.Navigate(imagePage);
                        }
                        else
                        {
                            FilePage filePage = new FilePage();
                            filePage.fileName.Content = filename.Split('\\')[filename.Split('\\').Length - 1];
                            filePage.openFileButton.Click += telegram.clickHandler.OpenFileButton_Click;
                            frame = new Frame();
                            frame.Navigate(filePage);
                        }
                        ShowFileMessage(user, frame);
                    }));
                }

            }
        }
    }
}
