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

namespace WpfApp11
{
    public class Config
    {
        public MainWindow mainWindow;

        public Config(MainWindow window)
        {
            mainWindow = window;
        }

        public void loadConfig()
        {
            //Add Icon to wpf project
            Bitmap bitmap = Resource1.Telegram_icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();
            mainWindow.Icon = Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap, IntPtr.Zero, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
            //#####################################################

            //Add start image for wpf poject
            IntPtr startBitmap = Resource1.Start.GetHbitmap();
            mainWindow.startImage.Source = Imaging.CreateBitmapSourceFromHBitmap(
                            startBitmap, IntPtr.Zero, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
            //#####################################################

            //Set config for Main Window
            mainWindow.Width = 781;
            mainWindow.Height = 793;
            mainWindow.MaxHeight = mainWindow.Height;
            mainWindow.MaxWidth = mainWindow.Width;
            mainWindow.MinHeight = mainWindow.Height;
            mainWindow.MinWidth = mainWindow.Width;
            //#####################################################

            //Set config for Sign In page
            mainWindow.signInFrame.Navigate(mainWindow.loginPage);
            mainWindow.loginPage.Visibility = Visibility.Hidden;
            mainWindow.loginPage.backButton.Click += mainWindow.clickHandler.Back_Click;
            mainWindow.loginPage.nextButton.Click += mainWindow.clickHandler.NextButton_Click;
            mainWindow.loginPage.password.TextChanged += mainWindow.stringHandler.TextChanged;
            mainWindow.loginPage.hideButton.Click += mainWindow.clickHandler.HideButtonLog_Click;
            mainWindow.loginPage.signUpButton.Click += mainWindow.clickHandler.SignUpButton_Click;
            mainWindow.startButton.Click += mainWindow.clickHandler.Start_Click;
            //#####################################################

            
            //Set config for Sign Up page
            mainWindow.signUpFrame.Navigate(mainWindow.registerPage);
            mainWindow.registerPage.Visibility = Visibility.Hidden;
            mainWindow.registerPage.backButton.Click += mainWindow.clickHandler.Back_Click;
            mainWindow.registerPage.password.TextChanged += mainWindow.stringHandler.TextChanged;
            mainWindow.registerPage.hideButton.Click += mainWindow.clickHandler.HideButtonReg_Click;
            mainWindow.registerPage.nextButton.Click += mainWindow.clickHandler.NextRegButton_Click;
            mainWindow.registerPage.name.TextChanged += mainWindow.stringHandler.DeleteErroreLabel;
            //#####################################################
        }


    }
}
