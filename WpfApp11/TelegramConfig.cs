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
    public class TelegramConfig
    {
        Telegram telegram;

        public TelegramConfig(Telegram t)
        {
            telegram = t;
        }

        public void Load()
        {
            telegram.udpChatClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            telegram.udpChatClient.Client.Bind(new IPEndPoint(IPAddress.Any,telegram.receivePort));
            telegram.udpChatClient.JoinMulticastGroup(IPAddress.Parse("224.5.5.5"));

            telegram.userInfoFrame.Navigate(telegram.userInfoPage);
            telegram.userInfoPage.Visibility = Visibility.Hidden;
            telegram.userInfoPage.userName.Content = telegram.user.name;
            telegram.mainFrame.Visibility = Visibility.Hidden;
            telegram.userInfoPage.addGroupButton.Click += telegram.clickHandler.AddNewGroupButton_Click;

            Bitmap bitmap = Resource1.Telegram_icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();
            telegram.Icon = Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap, IntPtr.Zero, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

            telegram.createGroupFrame.Navigate(telegram.addNewGroup);
            telegram.addNewGroup.createButton.Click += telegram.clickHandler.CreateGroupButton_Click;
            telegram.addNewGroup.Visibility = Visibility.Hidden;
            telegram.listGroupFrame.Navigate(telegram.groupList);
            telegram.groupList.Visibility = Visibility.Hidden;

            telegram.groupList.joinButton.Click += telegram.clickHandler.JoinButton_Click;
            

            Button button = new Button();
            button.Content = "";
            System.Windows.Media.Color color = new System.Windows.Media.Color();
            color.R = 0;
            color.G = 0;
            color.B = 0;

            

            SolidColorBrush brush = new SolidColorBrush(color);
            button.Background = brush;
            button.Background.Opacity = 35;
            button.Click += telegram.clickHandler.CloseInfoPage_Click;
            telegram.mainFrame.Navigate(button);

            ImageSource image = Imaging.CreateBitmapSourceFromHBitmap(
                            Resource1.chatPage.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
            ImageBrush imageBrush = new ImageBrush(image);
            telegram.chatPage.Background = imageBrush;

            image = Imaging.CreateBitmapSourceFromHBitmap(
                            Resource1.Voice.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
            imageBrush = new ImageBrush(image);

            telegram.fileButton.Click += telegram.clickHandler.fileButton_Click;

            telegram.sendButton.Click += telegram.clickHandler.sendButton_Click;
            telegram.voiceButton.Click += telegram.clickHandler.voiceButton_Click;
            telegram.infoPage.Click += telegram.clickHandler.InfoPageButton_Click;

            telegram.voiceButton.Background = imageBrush;

            telegram.listbox.SelectionChanged += telegram.clickHandler.SelectionChanged;

            telegram.joinGroupButton.Click += telegram.clickHandler.JoinGroupButton_Click;
        }

       
    }
}
