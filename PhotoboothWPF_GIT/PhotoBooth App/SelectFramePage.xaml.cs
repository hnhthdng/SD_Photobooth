using PhotoBooth_App.Model;
using PhotoBooth_App.Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoBooth_App
{
    public partial class SelectFramePage : Page
    {
        private int currentIndex = 0;
        public List<FrameItem> FrameList { get; set; } = new List<FrameItem>();
        private readonly ApiService _apiService;

        public SelectFramePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            DataContext = this;
            LoadFrames();
            UpdatePreview();
        }

        private async void LoadFrames()
        {
            try
            {
                var frames = await _apiService.GetAsync<List<FrameItem>>("/api/Frame");
                if (frames != null && frames.Any())
                {
                    FrameList = frames;
                    currentIndex = 0;
                    UpdatePreview();
                }
                else
                {
                    MessageBox.Show("Không có frame nào được tìm thấy.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải frame: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    
        private void UpdatePreview()
        {
            if (FrameList.Count > 0)
            {
                var frame = FrameList[currentIndex];
                LeftFrame.Source = new BitmapImage(new Uri(FrameList[(currentIndex - 1 + FrameList.Count) % FrameList.Count].FrameUrl, UriKind.RelativeOrAbsolute));
                PreviewFrame.Source = new BitmapImage(new Uri(frame.FrameUrl, UriKind.RelativeOrAbsolute));
                RightFrame.Source = new BitmapImage(new Uri(FrameList[(currentIndex + 1) % FrameList.Count].FrameUrl, UriKind.RelativeOrAbsolute));
                FrameName.Text = frame.Name.ToUpper();
                FrameDescription.Text = frame.Description;
            }
        }

        private void PreviousFrame_Click(object sender, RoutedEventArgs e)
        {
            currentIndex = (currentIndex - 1 + FrameList.Count) % FrameList.Count;
            UpdatePreview();
        }

        private void NextFrame_Click(object sender, RoutedEventArgs e)
        {
            currentIndex = (currentIndex + 1) % FrameList.Count;
            UpdatePreview();
        }

        private void SelectFrame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FrameList == null || FrameList.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy Frame.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                UserSession.SelectedFrame = FrameList[currentIndex];

                if (UserSession.UserRole == "Customer")
                {
                    NavigationService.Navigate(new TakePhotoScreen());
                }
                //else if (UserSession.UserRole == "Guest")
                //{
                //    NavigationService.Navigate(new QuantitySelectionPage());
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi : " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
