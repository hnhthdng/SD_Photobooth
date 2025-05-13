using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using PhotoBooth_App.Handler;
using PhotoBooth_App.Service;
using PhotoBooth_App.Services;
using PhotoBooth_App.Model;

namespace PhotoBooth_App
{
    public partial class CheckPhotoPage : Page
    {
        private string selectedImagePath;
        private List<BitmapImage> images = new List<BitmapImage>();
        private int currentIndex = 0;
        private BitmapImage frameImage;
        private string historyFolderPath;
        string baseUrl = AppConfig.GetApiUrl();
        private readonly ApiService _apiService;

        public CheckPhotoPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            string solutionRoot = GetPhotoHandler.FindSolutionRoot();
            historyFolderPath = Path.Combine(solutionRoot, "PhotoBooth App", "history");
            Loaded += CheckPhotoPage_Loaded;
        }

        private async void CheckPhotoPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadFrame();
            LoadImages();
        }

        private async Task LoadFrame()
        {
            if (UserSession.SelectedFrame != null)
            {
                try
                {
                    FrameItem frameResponse = await _apiService.GetAsync<FrameItem>($"/api/frame/{UserSession.SelectedFrame.Id}");
                    if (frameResponse != null && !string.IsNullOrEmpty(frameResponse.FrameUrl))
                    {
                        UserSession.SelectedFrame = frameResponse;
                        frameImage = new BitmapImage();
                        frameImage.BeginInit();
                        frameImage.UriSource = new Uri(frameResponse.FrameUrl, UriKind.Absolute);
                        frameImage.CacheOption = BitmapCacheOption.OnLoad;
                        frameImage.EndInit();

                        MainImage.Source = frameImage;
                    }
                    else
                    {
                        MessageBox.Show("Dữ liệu frame từ API không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi gọi API frame: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Chưa có frame nào được chọn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void LoadImages()
        {
            images.Clear();

            foreach (var file in TakePhotoScreen.CurrentImages)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(file, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                images.Add(bitmap);
            }

            ImageList.ItemsSource = images;

            if (images.Count > 0)
            {
                SetMainImage(0);
            }
        }
        private void SetMainImage(int index)
        {
            if (index < 0 || index >= images.Count || frameImage == null)
                return;

            int slotCount = UserSession.SelectedFrame.SlotCount;
            var coords = UserSession.SelectedFrame.Coordinates;

            if (slotCount == 1)
            {
                var merged = FrameHandler.ApplyFrameToImages(
                    new List<BitmapImage> { images[index] },
                    frameImage,
                    coords);
                MainImage.Source = merged;
                selectedImagePath = images[index].UriSource.LocalPath;
                currentIndex = index;
                ImageList.SelectedIndex = index;
            }
            else
            {
                var selectedImgs = images.Skip(index).Take(slotCount).ToList();
                var merged = FrameHandler.ApplyFrameToImages(
                    selectedImgs,
                    frameImage,
                    coords);
                MainImage.Source = merged;
                selectedImagePath = selectedImgs[0].UriSource.LocalPath;
                currentIndex = index;
                ImageList.SelectedIndex = index;
            }
        }

        private void ImageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageList.SelectedIndex >= 0)
            {
                SetMainImage(ImageList.SelectedIndex);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveAllImagesWithFrame();
        }

        private void SaveAllImagesWithFrame()
        {
            if (frameImage == null)
            {
                MessageBox.Show("Chưa có frame nào được chọn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (images.Count == 0)
            {
                MessageBox.Show("Không có ảnh nào để chỉnh sửa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string outputFolder = Path.Combine(historyFolderPath, "framed_images");
            Directory.CreateDirectory(outputFolder);

            int slotCount = UserSession.SelectedFrame.SlotCount;
            var coords = UserSession.SelectedFrame.Coordinates;
            List<BitmapImage> newImages = new List<BitmapImage>();

            if (slotCount == 1)
            {
                foreach (var img in images)
                {
                    var merged = FrameHandler.ApplyFrameToImages(new List<BitmapImage> { img }, frameImage, coords);
                    string newImagePath = Path.Combine(outputFolder, $"{slotCount}_framed_{Guid.NewGuid()}.png");

                    using (FileStream stream = new FileStream(newImagePath, FileMode.Create))
                    {
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(merged));
                        encoder.Save(stream);
                    }

                    TakePhotoScreen.CurrentImages.Add(newImagePath);

                    BitmapImage newImg = new BitmapImage();
                    newImg.BeginInit();
                    newImg.UriSource = new Uri(newImagePath, UriKind.Absolute);
                    newImg.CacheOption = BitmapCacheOption.OnLoad;
                    newImg.EndInit();
                    newImages.Add(newImg);
                }
            }
            else
            {
                int index = 0;
                while (index < images.Count)
                {
                    var selectedImages = images.Skip(index).Take(slotCount).ToList();
                    if (selectedImages.Count == 0) break;

                    var merged = FrameHandler.ApplyFrameToImages(selectedImages, frameImage, coords);
                    string newImagePath = Path.Combine(outputFolder, $"{slotCount}_framed_{index / slotCount + 1}.png");

                    using (FileStream stream = new FileStream(newImagePath, FileMode.Create))
                    {
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(merged));
                        encoder.Save(stream);
                    }

                    TakePhotoScreen.CurrentImages.Add(newImagePath);

                    BitmapImage newImg = new BitmapImage();
                    newImg.BeginInit();
                    newImg.UriSource = new Uri(newImagePath, UriKind.Absolute);
                    newImg.CacheOption = BitmapCacheOption.OnLoad;
                    newImg.EndInit();
                    newImages.Add(newImg);

                    index += slotCount;
                }
            }

            images.AddRange(newImages);
            images = newImages;
            ImageList.ItemsSource = images;

            MessageBox.Show($"Ảnh đã được lưu", "Lưu Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Sticker_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedImagePath))
            {
                MessageBox.Show("Vui lòng chọn một ảnh trước!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            NavigationService.Navigate(new StickerEditorPage(selectedImagePath));
        }

        private void ScrollUp(object sender, RoutedEventArgs e)
        {
            if (currentIndex > 0)
            {
                SetMainImage(currentIndex - 1);
            }
        }

        private void ScrollDown(object sender, RoutedEventArgs e)
        {
            if (currentIndex < images.Count - 1)
            {
                SetMainImage(currentIndex + 1);
            }
        }


        private async Task<bool> UploadImagesToServer()
        {
            if (TakePhotoScreen.CurrentImages == null || !TakePhotoScreen.CurrentImages.Any())
            {
                MessageBox.Show("Không có ảnh nào để tải lên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            string sessionCode = UserSession.SessionCode;
            if (string.IsNullOrEmpty(sessionCode))
            {
                MessageBox.Show("Không tìm thấy mã phiên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StringContent(sessionCode), "SessionCode");

                        foreach (var imagePath in TakePhotoScreen.CurrentImages)
                        {
                            if (!File.Exists(imagePath))
                                continue;

                            byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
                            var fileContent = new ByteArrayContent(imageBytes);

                            string extension = Path.GetExtension(imagePath).ToLower();
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
                                Name = "\"PhotoPaths\"",
                                FileName = $"\"{Path.GetFileName(imagePath)}\""
                            };

                            content.Add(fileContent, "PhotoPaths", Path.GetFileName(imagePath));
                        }

                        var response = await client.PostAsync($"{baseUrl}/api/PhotoHistory/upload", content);
                        string responseContent = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Ảnh đã được tải lên và lưu thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show($"Lỗi khi tải ảnh lên: {response.ReasonPhrase}\nChi tiết: {responseContent}",
                                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }


        private async void next_Click(object sender, RoutedEventArgs e)
        {
            bool uploadSuccess = await UploadImagesToServer();

            if (!uploadSuccess)
            {
                return;
            }

            if (string.IsNullOrEmpty(selectedImagePath))
            {
                MessageBox.Show("Vui lòng chọn một ảnh trước!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NavigationService.Navigate(new PhotoDetailPage(selectedImagePath));
        }
    }
}