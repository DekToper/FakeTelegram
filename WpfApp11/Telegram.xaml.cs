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
using System.Diagnostics;

namespace WpfApp11
{
    /// <summary>
    /// Логика взаимодействия для Telegram.xaml
    /// </summary>
    public partial class Telegram : Window
    {
        public class DirectoryListing
        {
            public string Name { get; set; }
            public System.Windows.Controls.Image Image { get; set; }
        }

        public ServerConect serverConect;
        public User user = new User();
        public UserInfo userInfoPage = new UserInfo();
        public bool record = false;
        public WaveFileWriter writer;
        public WaveFileReader fileReader;

        public Load loadPage = new Load();

        public UdpClient udpChatClient = new UdpClient();

        public int receivePort = 9001;

        public WaveIn sourceStream = new WaveIn();
        public DirectSoundOut waveOut = new DirectSoundOut();
        public Thread thread;
        public Thread threadLarge;

        public TelegramConfig config;
        public ClickHandlerTelegram clickHandler;
        public ChatHandler chatHandler;
        public AddNewGroup addNewGroup = new AddNewGroup();

        public GroupList groupList = new GroupList();

        public Telegram(User u, ServerConect server)
        {
            
            InitializeComponent();
            user = u;
            serverConect = server;

            chatHandler = new ChatHandler(this);
            clickHandler = new ClickHandlerTelegram(this);
            config = new TelegramConfig(this);
            config.Load();
            
            chatHandler.LoadGroups();

            chatHandler.ShowMessage("Telegram", "Please select channel to continue...");

            thread = new Thread(chatHandler.getMessage);
            thread.Start();

        }


     
        ~Telegram()
        {
            
        }
    
    }
}
