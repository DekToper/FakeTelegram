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
using System.Windows.Forms;
using System.Diagnostics;

namespace WpfApp11
{
    public class ClickHandlerTelegram
    {
        Telegram telegram;

        public ClickHandlerTelegram(Telegram t)
        {
            telegram = t;
        }

        internal void InfoPageButton_Click(object sender, RoutedEventArgs e)
        {
            telegram.mainFrame.Visibility = Visibility.Visible;
            telegram.userInfoPage.Visibility = Visibility.Visible;
            DoubleAnimation widthAnimation = new DoubleAnimation(0, 235, TimeSpan.FromSeconds(0.1));
            telegram.userInfoPage.BeginAnimation(Page.WidthProperty, widthAnimation);
        }

        internal void CloseInfoPage_Click(object sender, RoutedEventArgs e)
        {
            telegram.mainFrame.Visibility = Visibility.Hidden;
            DoubleAnimation widthAnimation = new DoubleAnimation(235, 0, TimeSpan.FromSeconds(0.1));
            telegram.userInfoPage.BeginAnimation(Page.WidthProperty, widthAnimation);
            telegram.chatHandler.HidePage(telegram.userInfoPage);

            widthAnimation = new DoubleAnimation(320, 0, TimeSpan.FromSeconds(0.1));
            telegram.groupList.BeginAnimation(Page.WidthProperty, widthAnimation);
            telegram.chatHandler.HidePage(telegram.groupList);
            telegram.groupList.listGroup.Items.Clear();

            widthAnimation = new DoubleAnimation(320, 0, TimeSpan.FromSeconds(0.1));
            telegram.addNewGroup.BeginAnimation(Page.WidthProperty, widthAnimation);
            telegram.chatHandler.HidePage(telegram.addNewGroup);
        }

        internal void CreateGroupButton_Click(object sender, RoutedEventArgs e)
        {
            string groupName = telegram.addNewGroup.grupNameBox.Text;
            IPEndPoint iPEndPoint = new IPEndPoint(telegram.serverConect.groupAddress, telegram.serverConect.sendPort);
            byte[] data = Encoding.Default.GetBytes("(" + telegram.user.name + " " + groupName);
            telegram.chatHandler.Send(data, iPEndPoint);
            telegram.chatHandler.AddGroup(groupName);
            DoubleAnimation widthAnimation = new DoubleAnimation(320, 0, TimeSpan.FromSeconds(0.1));
            telegram.addNewGroup.BeginAnimation(Page.WidthProperty, widthAnimation);
            telegram.chatHandler.HidePage(telegram.addNewGroup);
            telegram.mainFrame.Visibility = Visibility.Hidden;  
        }

        internal void AddNewGroupButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation widthAnimation = new DoubleAnimation(235, 0, TimeSpan.FromSeconds(0.1));
            telegram.userInfoPage.BeginAnimation(Page.WidthProperty, widthAnimation);
            telegram.chatHandler.HidePage(telegram.userInfoPage);
            telegram.addNewGroup.Visibility = Visibility.Visible;
            widthAnimation = new DoubleAnimation(0, 320, TimeSpan.FromSeconds(0.1));
            telegram.addNewGroup.BeginAnimation(Page.WidthProperty, widthAnimation);
        }

        internal void JoinButton_Click(object sender, RoutedEventArgs e)
        {

            if(telegram.groupList.listGroup.SelectedValue != null)
            {
                string groupName = (telegram.groupList.listGroup.SelectedValue as System.Windows.Controls.ListViewItem).Content.ToString();
                IPEndPoint iPEndPoint = new IPEndPoint(telegram.serverConect.groupAddress, telegram.serverConect.sendPort);
                byte[] data = Encoding.Default.GetBytes("^" + telegram.user.name + ' ' + groupName);
                telegram.chatHandler.Send(data, iPEndPoint);
                telegram.chatHandler.AddGroup(groupName);
                DoubleAnimation widthAnimation = new DoubleAnimation(320, 0, TimeSpan.FromSeconds(0.1));
                telegram.groupList.BeginAnimation(Page.WidthProperty, widthAnimation);
                telegram.chatHandler.HidePage(telegram.groupList);
                telegram.groupList.listGroup.Items.Clear();
                telegram.mainFrame.Visibility = Visibility.Hidden;
            }

        }

        internal void JoinGroupButton_Click(object sender, RoutedEventArgs e)
        {
            telegram.chatHandler.GetAllGroups();
            telegram.mainFrame.Visibility = Visibility.Visible;
            telegram.groupList.Visibility = Visibility.Visible;
            DoubleAnimation widthAnimation = new DoubleAnimation(0, 320, TimeSpan.FromSeconds(0.1));
            telegram.groupList.BeginAnimation(Page.WidthProperty, widthAnimation);
        }


        internal void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if (telegram.record)
            {
                telegram.voiceButton.IsEnabled = true;
                telegram.sourceStream.StopRecording();
                telegram.sourceStream.Dispose();
                telegram.sourceStream = null;
                telegram.writer.Close();
                telegram.writer.Dispose();
                telegram.threadLarge = new Thread(SendLargeFile);
                telegram.threadLarge.Start(telegram.writer.Filename);

                telegram.record = false;
            }
            else
            {
                byte[] data = Encoding.Default.GetBytes("#");
                telegram.chatHandler.Send(data, new IPEndPoint(IPAddress.Parse("224.5.5.5"), telegram.receivePort));
                data = Encoding.Default.GetBytes((telegram.listbox.SelectedItem as Frame).Name + "/" + telegram.user.name + "/" + telegram.chatBox.Text);
                telegram.chatHandler.Send(data, new IPEndPoint(IPAddress.Parse("224.5.5.5"), telegram.receivePort));
                telegram.chatBox.Text = "";
            }
        }

        internal void fileButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "";
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "txt files (*.txt)|*.txt|document files (*.docx)|*.docx|All files (*.*)|*.*";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = fileDialog.FileName;
                    telegram.threadLarge = new Thread(SendLargeFile);
                    telegram.threadLarge.Start(filePath);
                }
            }
        }

        

        internal void playButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Directory.GetCurrentDirectory() + "\\..\\..\\Records\\" + (((sender as System.Windows.Controls.Button).Parent as Grid).Parent as Wave).musicName.Content.ToString();
            telegram.fileReader = new WaveFileReader(filePath);

            telegram.waveOut = new DirectSoundOut();
            telegram.waveOut.Init(new WaveChannel32(telegram.fileReader));
            telegram.waveOut.Play();
        }

        internal void voiceButton_Click(object sender, RoutedEventArgs e)
        {
            string saveLocation = "../../Resources/LastRecord.wav";

            telegram.sourceStream = new WaveIn();
            telegram.sourceStream.DeviceNumber = 0;
            telegram.sourceStream.WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(0).Channels);
            if (telegram.fileReader != null)
            {
                telegram.fileReader.Close();
                telegram.fileReader.Dispose();
            }
            telegram.sourceStream.DataAvailable += new EventHandler<WaveInEventArgs>(sourceStream_DataAvailable);
            telegram.writer = new WaveFileWriter(saveLocation, telegram.sourceStream.WaveFormat as WaveFormat);
            telegram.record = true;
            telegram.sourceStream.StartRecording();

        }
        private void sourceStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (telegram.writer == null) return;

            telegram.writer.WriteData(e.Buffer, 0, e.BytesRecorded);
            telegram.writer.Flush();
        }

        internal void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            telegram.chatPage.Items.Clear();
            telegram.groupName.Content = (telegram.listbox.SelectedItem as Frame).Name;
        }

        private void SendLargeFile(object filePath)
        {
            byte[] data = null;
            DoubleAnimation widthAnimation;
            telegram.Dispatcher.Invoke(new Action(() =>
            {
                telegram.loadPage.Visibility = Visibility.Hidden;
                telegram.LoadFileFrame.Navigate(telegram.loadPage);

                data = Encoding.Default.GetBytes("@");
                telegram.chatHandler.Send(data, new IPEndPoint(IPAddress.Parse("224.5.5.5"), telegram.receivePort));
                data = Encoding.Default.GetBytes(telegram.user.name);
                telegram.chatHandler.Send(data, new IPEndPoint(IPAddress.Parse("224.5.5.5"), telegram.receivePort));
                data = File.ReadAllBytes(filePath as string);
                byte[] dataLength = Encoding.Default.GetBytes(data.Length.ToString());
                telegram.chatHandler.Send(dataLength, new IPEndPoint(IPAddress.Parse("224.5.5.5"), telegram.receivePort));

                telegram.loadPage.Visibility = Visibility.Visible;
                widthAnimation = new DoubleAnimation(0, 260, TimeSpan.FromSeconds(0.1));
                telegram.loadPage.BeginAnimation(Page.WidthProperty, widthAnimation);
            }));
            int k = FileHandler.GetFilePartsNumber(data.Length);
            for (int i = 0; i < k + 1; i++)
            {
                telegram.Dispatcher.Invoke(new Action(() =>
                {
                    byte[] buf = FileHandler.GetFilePart(i, data);
                    telegram.chatHandler.Send(buf, new IPEndPoint(IPAddress.Parse("224.5.5.5"), telegram.receivePort));

                    telegram.loadPage.progressBar.Value += 100 / (k + 1);
                }));

                Thread.Sleep(1000);
            }
            telegram.Dispatcher.Invoke(new Action(() =>
            {
                telegram.loadPage.progressBar.Value = 100;
            }));

            Thread.Sleep(1000);

            telegram.Dispatcher.Invoke(new Action(() =>
            {
                data = Encoding.Default.GetBytes('.' + (filePath as string).Split('.')[(filePath as string).Split('.').Length - 1]);
                telegram.chatHandler.Send(data, new IPEndPoint(IPAddress.Parse("224.5.5.5"), telegram.receivePort));
                telegram.loadPage.progressBar.Value = 0;
                widthAnimation = new DoubleAnimation(260, 0, TimeSpan.FromSeconds(0.1));
                telegram.loadPage.BeginAnimation(Page.WidthProperty, widthAnimation);

                telegram.chatHandler.HidePage(telegram.loadPage);
            }));

        }

        internal void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Directory.GetCurrentDirectory() + "\\..\\..\\Records\\" + (((sender as System.Windows.Controls.Button).Parent as Grid).Parent as FilePage).fileName.Content.ToString();
            Process.Start(filePath);
        }
    }
}
