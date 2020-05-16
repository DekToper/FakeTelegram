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
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WpfApp11
{
    public class ClickHandler
    {
        public string password = "";
        public bool hide { get; protected set; }
        MainWindow mainWindow;
        public bool change { get; set; }
        ServerConect serverConect;

        public ClickHandler(MainWindow window)
        {
            mainWindow = window;
            hide = true;
            change = true;
            serverConect = new ServerConect();
        }

        internal void HideButtonLog_Click(object sender, RoutedEventArgs e)
        {
            string buffer = "";
            if (hide)
            {
                hide = false;
                (((sender as Button).Parent as Grid).Parent as Login).password.Text = password;
            }
            else
            {

                hide = true;
                change = false;
                for (int i = 0; i < (((sender as Button).Parent as Grid).Parent as Login).password.Text.Length; i++)
                {
                    buffer += '*';
                }
                (((sender as Button).Parent as Grid).Parent as Login).password.Text = buffer;
            }
        }


        internal void NextRegButton_Click(object sender, RoutedEventArgs e)
        {
            if(password == mainWindow.registerPage.confirm_password.Text)
            {
                mainWindow.user.name = mainWindow.registerPage.name.Text;
                mainWindow.user.SetPassword(password);

                if (serverConect.SendUserDetails(mainWindow.user.name + " " + mainWindow.user.GetPassword(), '$'))
                {
                    mainWindow.user.port = serverConect.receivePort;
                    Telegram telegram = new Telegram(mainWindow.user,serverConect);
                    mainWindow.Visibility = Visibility.Hidden;
                    telegram.Owner = mainWindow;
                    telegram.ShowDialog();
                    mainWindow.Close();
                }
                else
                {
                    mainWindow.registerPage.erroreLabel.Content = "Name already exist!";
                }
            }
            else
            {
                mainWindow.registerPage.erroreLabel.Content = "Passwords do not match.";
            }
        }


        internal void HideButtonReg_Click(object sender, RoutedEventArgs e)
        {
            string buffer = "";
            if (hide)
            {
                hide = false;
                (((sender as Button).Parent as Grid).Parent as Register).password.Text = password;
            }
            else
            {

                hide = true;
                change = false;
                for (int i = 0; i < (((sender as Button).Parent as Grid).Parent as Register).password.Text.Length; i++)
                {
                    buffer += '*';
                }
                (((sender as Button).Parent as Grid).Parent as Register).password.Text = buffer;
            }
        }

        internal void NextButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.user.name = mainWindow.loginPage.name.Text;
            mainWindow.user.SetPassword(password);
            if(serverConect.SendUserDetails(mainWindow.user.name + " " + mainWindow.user.GetPassword(), '@'))
            {
                
                mainWindow.user.port = serverConect.receivePort;
                Telegram telegram = new Telegram(mainWindow.user, serverConect);
                
                mainWindow.Visibility = Visibility.Hidden;
                telegram.Owner = mainWindow;
                telegram.ShowDialog();
                mainWindow.Close();

            }
            else
            {
                mainWindow.loginPage.erroreLabel.Content = "Wrong password or login!";
            }
        }

        public void HidePage(UIElement element)
        {
            Thread thread = new Thread(Hide);
            thread.Start(element);
        }

        private void Hide(object obj)
        {
            UIElement element = obj as UIElement;
            Thread.Sleep(200);
            SetVisibility(element, Visibility.Hidden);
            Thread.CurrentThread.Abort();
        }

        public void SetVisibility(UIElement element,Visibility v)
        {
            mainWindow.Dispatcher.Invoke(new Action(() =>
            {
                element.Visibility = v;
            }
            ));
        }

        public void clearData()
        {
            password = "";
            hide = true;
            change = true;

            mainWindow.loginPage.password.Text = "";
            mainWindow.loginPage.name.Text = "";
            mainWindow.loginPage.erroreLabel.Content = "";

            mainWindow.registerPage.password.Text = "";
            mainWindow.registerPage.name.Text = "";
            mainWindow.registerPage.erroreLabel.Content = "";

        }

        internal void Back_Click(object sender, RoutedEventArgs e)
        {
            clearData();
            DoubleAnimation heightAnimation = new DoubleAnimation(761, 0, TimeSpan.FromSeconds(0.2));
            (((sender as Button).Parent as Grid).Parent as Page).BeginAnimation(Page.HeightProperty, heightAnimation);
            HidePage(((sender as Button).Parent as Grid).Parent as Page);
        }

        internal void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            clearData();
            mainWindow.registerPage.Visibility = Visibility.Visible;
            DoubleAnimation widthAnimation = new DoubleAnimation(0, 761, TimeSpan.FromSeconds(0.2));
            mainWindow.registerPage.BeginAnimation(Login.WidthProperty, widthAnimation);
        }

        internal void Start_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.loginPage.Visibility = Visibility.Visible;
            DoubleAnimation heightAnimation = new DoubleAnimation(500, 761, TimeSpan.FromSeconds(0.2));
            mainWindow.loginPage.BeginAnimation(Login.HeightProperty,heightAnimation);
        }

    }
}
