using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// <summary>
    /// Логика взаимодействия для Group.xaml
    /// </summary>
    public partial class Group : Page
    {
        public Group(string resource)
        {
            InitializeComponent();
            if (resource == "Group")
            {
                IntPtr groupBitmap = Resource1.png.GetHbitmap();
                GroupImage.Source = Imaging.CreateBitmapSourceFromHBitmap(
                                groupBitmap, IntPtr.Zero, Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());
            }
            else if (resource == "User")
            {
                IntPtr groupBitmap = Resource1.userPng.GetHbitmap();
                GroupImage.Source = Imaging.CreateBitmapSourceFromHBitmap(
                                groupBitmap, IntPtr.Zero, Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());
            }
        }

    }
}
