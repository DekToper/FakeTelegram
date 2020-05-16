using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;

namespace WpfApp11
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Pages
        public Login loginPage = new Login();
        public Register registerPage = new Register();
        //############################################

        //Handlers
        public ClickHandler clickHandler;
        public StringHandler stringHandler;
        //############################################

        //User's classes
        public User user = new User();
        //############################################

        public MainWindow()
        {
            InitializeComponent();

            stringHandler = new StringHandler(this);
            clickHandler = new ClickHandler(this);

            Config config = new Config(this);
            config.loadConfig();
            
        }

        ~MainWindow()
        {
            Process.GetProcessesByName("WpfApp11.exe")[0].Kill();
            Process.GetCurrentProcess().Kill();
        }
    }
}