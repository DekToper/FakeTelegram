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
    public class StringHandler
    {

        bool blockAddNewSymbol = false;
        MainWindow mainWindow;

        public StringHandler(MainWindow window)
        {
            mainWindow = window;
        }


        internal void TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text.Length > 16)
            {
                blockAddNewSymbol = true;

            }
            else if ((sender as TextBox).Text.Length == 16)
            {
                mainWindow.loginPage.erroreLabel.Content = "Your password can’t be longer than 16 characters";
            }
            else
            {
                mainWindow.loginPage.erroreLabel.Content = "";
            }


            if (!blockAddNewSymbol)
                try
                {
                    string buffer = "";
                    if ((sender as TextBox).Text[(sender as TextBox).Text.Length - 1] != '*')
                    {
                        if (mainWindow.clickHandler.hide)
                        {

                            buffer = mainWindow.clickHandler.password + (sender as TextBox).Text.Replace("*", "");
                            (sender as TextBox).Text = (sender as TextBox).Text.Replace((sender as TextBox).Text[(sender as TextBox).Text.Length - 1], '*');
                            Thread.Sleep(50);
                            mainWindow.clickHandler.password = buffer;
                            (sender as TextBox).CaretIndex = (sender as TextBox).Text.Length;
                        }
                        else
                        {
                            if (mainWindow.clickHandler.password.Length < (sender as TextBox).Text.Length)
                            {
                                mainWindow.clickHandler.password += (sender as TextBox).Text[(sender as TextBox).Text.Length - 1];
                            }
                            else if (mainWindow.clickHandler.password.Length > (sender as TextBox).Text.Length)
                            {
                                mainWindow.clickHandler.password = DeleteLastSymbol(mainWindow.clickHandler.password);
                            }
                        }
                    }
                    else
                    {
                        if (mainWindow.clickHandler.change)
                        {
                            mainWindow.clickHandler.password = DeleteLastSymbol(mainWindow.clickHandler.password);
                        }
                        else
                        {
                            mainWindow.clickHandler.change = true;
                        }

                    }
                }
                catch
                {
                    mainWindow.clickHandler.password = "";
                }
            else
            {
                if (mainWindow.clickHandler.password.Length != (sender as TextBox).Text.Length)
                {
                    (sender as TextBox).Text = DeleteLastSymbol((sender as TextBox).Text);
                    (sender as TextBox).CaretIndex = (sender as TextBox).Text.Length;
                    blockAddNewSymbol = false;
                }
            }

        }

        internal void DeleteErroreLabel(object sender, TextChangedEventArgs e)
        {
            mainWindow.registerPage.erroreLabel.Content = "";
        }

        public string DeleteLastSymbol(string text)
        {
            string buffer = "";
            for (int i = 0; i < text.Length - 1; i++)
            {
                buffer += text[i];
            }
            return buffer;
        }



    }
}
