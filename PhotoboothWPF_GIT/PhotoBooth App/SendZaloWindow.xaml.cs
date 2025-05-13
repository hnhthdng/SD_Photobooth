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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhotoBooth_App
{
    /// <summary>
    /// Interaction logic for SendZaloWindow.xaml
    /// </summary>
    public partial class SendZaloWindow : Window
    {
        public SendZaloWindow()
        {
            InitializeComponent();
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn tiếp tục không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                UserSession.Logout();
                TakePhotoScreen.CurrentImages.Clear();
                Login login = new Login();
                login.Show();
                Application.Current.MainWindow = login;

                foreach (Window window in Application.Current.Windows)
                {
                    if (window != login)
                    {
                        window.Close();
                    }
                }
            }
        }

    }
}
