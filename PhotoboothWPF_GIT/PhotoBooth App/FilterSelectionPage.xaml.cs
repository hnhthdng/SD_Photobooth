using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using PhotoBooth_App.Model;
using PhotoBooth_App.Service;
using PhotoBooth_App.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public partial class FilterSelectionPage : Page
    {
        private string selectedImage;
        private readonly ApiService _apiService;
        private int currentIndex = 0;
        public List<PhotoStyleItem> style = new List<PhotoStyleItem>();
        string baseUrl = AppConfig.GetApiUrl();
        private Border selectedBorder = null;



        public FilterSelectionPage(string selectedImage)
        {
            InitializeComponent();
            _apiService = new ApiService();
            this.selectedImage = selectedImage;

            Loaded += FilterSelectionPage_Loaded;
            //        ShowProcessedImage([]);
        }

        private async void FilterSelectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (style.Count == 0) await LoadFilterImages();
          //   UpdatePreview();
            DisplayImage();
            InitializeSignalR();
        }

        private void DisplayImage()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(selectedImage, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            SelectedImage.Source = bitmap;
        }

        private async Task LoadFilterImages()
        {
            try
            {
                List<PhotoStyleItem> frames = await _apiService.GetAsync<List<PhotoStyleItem>>("/api/PhotoStyle");

                if (frames != null && frames.Count > 0)
                {
                    style = frames;
                    StyleList.ItemsSource = style;
                }
                else
                {
                    MessageBox.Show("Không có kiểu ảnh nào được tìm thấy.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy danh sách kiểu ảnh: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void StyleItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is PhotoStyleItem selectedStyle)
            {
                if (selectedBorder != null)
                {
                    selectedBorder.Background = Brushes.Transparent;
                }
                selectedBorder = border;
                selectedBorder.Background = Brushes.LightBlue;
                currentIndex = style.IndexOf(selectedStyle);
                //  UpdatePreview();
            }
        }

        private void Save_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (style.Count > 0)
                {
                    var selectedStyle = style[currentIndex];
                    int photoStyle = selectedStyle.id;
                    //       BorderEffect.BorderBrush = new SolidColorBrush(Colors.Blue);
                    ExecuteImage(selectedImage, photoStyle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi: " + ex.Message);
            }
        }


        private async void InitializeSignalR()
        {
            await TranformPhotoHub.Instance.StartConnectionAsync();

            TranformPhotoHub.Instance.Connection.On<string[]>("ReceiveProcessedPhoto", (response) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (response != null && response.Length > 0)
                    {
                        Console.WriteLine(response);

                        ShowProcessedImage(response);
                    }
                    else
                    {
                        MessageBox.Show("Không có ảnh được trả về");
                    }
                });
            });
        }

        private void ShowProcessedImage(string[] imageUrl)
        {
            try
            {
                var imageList = new List<BitmapImage>();
                foreach (var image in imageUrl)
                {

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(image, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    imageList.Add(bitmap);
                    TakePhotoScreen.CurrentImages.Add(image);

                }

                //ImagesLists.ItemsSource = imageList;
                OpenProcessedImageWindow(imageList);
                Console.WriteLine("Hiển thị ảnh thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}");
            }
        }

        private void OpenProcessedImageWindow(List<BitmapImage> processedImages)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListPhotoEdited photoWindow = new ListPhotoEdited(processedImages);
                photoWindow.ShowDialog();
            });
        }


        private async void ExecuteImage(string imagePath, int photoStyle)
        {
            await TranformPhotoHub.Instance.RegisterSessionAsync(UserSession.SessionCode);
            var (isValid, connectionId) = await TranformPhotoHub.Instance.RegisterAsync("");
            try
            {
                using (var client = new HttpClient())
                using (var form = new MultipartFormDataContent())
                {
                    form.Add(new StringContent(photoStyle.ToString()), "photoStyleId");
                    form.Add(new StringContent(TranformPhotoHub.connectionId), "connectionId");
                    form.Add(new StringContent(UserSession.SessionCode), "sessionCode");

                    byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
                    var fileContent = new ByteArrayContent(imageBytes);

                    string extension = System.IO.Path.GetExtension(imagePath).ToLower();
                    string contentType = extension switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".webp" => "image/webp",
                        ".bmp" => "image/bmp",
                        ".gif" => "image/gif",
                        _ => "application/octet-stream"
                    };

                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "\"image\"",
                        FileName = $"\"{System.IO.Path.GetFileName(imagePath)}\""
                    };

                    form.Add(fileContent, "image", System.IO.Path.GetFileName(imagePath));

                    var response = await client.PostAsync($"{baseUrl}/api/ImageProcessing", form);

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"StatusCode: {response.StatusCode} - Response: {responseContent}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi: " + ex.Message);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            ExecuteImage(selectedImage, style[currentIndex].id);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TakePhotoScreen());
        }
    }
}
