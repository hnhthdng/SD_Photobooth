using PhotoBooth_App.Model;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Path = System.IO.Path;

namespace PhotoBooth_App
{
    /// <summary>
    /// Interaction logic for PhotoSelectedPage.xaml
    /// </summary>
    public partial class PhotoSelectedPage : Page
    {
        private string _imagePath;
        public static PhotoSelectedPageReturnData ReturnData { get; set; } = new PhotoSelectedPageReturnData();

        public PhotoSelectedPage(string imagePath)
        {
            InitializeComponent();
            _imagePath = imagePath;
            DetailImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            ReturnData.ImagePath = imagePath;
        }

        private void TakePhoto_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TakePhotoScreen());
        }

        private void DeletePhoto_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = Uri.UnescapeDataString(_imagePath);
            string directoryPath = Path.GetDirectoryName(imagePath);
            string fileName = Path.GetFileName(imagePath);

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
            else
            {
                MessageBox.Show("The image file does not exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Work with the log file to remove the path
            string logFilePath = Path.Combine(directoryPath, "download_log.txt");
            if (!File.Exists(logFilePath))
            {
                MessageBox.Show("The log file does not exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var lines = File.ReadAllLines(logFilePath).ToList();
            lines.RemoveAll(line => line.Contains(imagePath));
            File.WriteAllLines(logFilePath, lines);

            TakePhotoScreen.CurrentImages.Remove(imagePath);

            ReturnData.IsDeleted = true; MessageBox.Show("Ảnh đã được xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();

            }


        }

        private void EditPhoto_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FilterSelectionPage(_imagePath));
        }
    }

}
