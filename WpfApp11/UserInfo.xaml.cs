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
    /// Логика взаимодействия для UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Page
    {
        public UserInfo()
        {
            InitializeComponent();

            IntPtr userInfoBitmap = Resource1.UserDataImage.GetHbitmap();
            userDataImage.Source = Imaging.CreateBitmapSourceFromHBitmap(
                            userInfoBitmap, IntPtr.Zero, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

            IntPtr groupBitmap = Resource1.Group.GetHbitmap();
            groupImage.Source = Imaging.CreateBitmapSourceFromHBitmap(
                            groupBitmap, IntPtr.Zero, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

            IntPtr sentingsBitmap = Resource1.Seting.GetHbitmap();
            setingsImage.Source = Imaging.CreateBitmapSourceFromHBitmap(
                            sentingsBitmap, IntPtr.Zero, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());

            IntPtr contactsBitmap = Resource1.Contacts.GetHbitmap();
            contactsImage.Source = Imaging.CreateBitmapSourceFromHBitmap(
                            contactsBitmap, IntPtr.Zero, Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
            
            
        }

        private void groupButton_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
