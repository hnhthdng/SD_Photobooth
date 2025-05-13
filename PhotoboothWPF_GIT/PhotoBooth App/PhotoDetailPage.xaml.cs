using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PhotoBooth_App.Handler;
using PhotoBooth_App.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoBooth_App
{
    /// <summary>
    /// Interaction logic for PhotoDetailPage.xaml
    /// </summary>
    public partial class PhotoDetailPage : Page, IBackButton
    {
        public bool ShowBackButton => true;

        private string imagePath;
        private MainWindow1 mainwindow;
        string baseUrl = AppConfig.GetApiUrl();

        public PhotoDetailPage(string imagePath)
        {
            InitializeComponent();
            this.imagePath = imagePath;
          DisplayImage();
        }

        private void DisplayImage()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            DetailImage.Source = bitmap;
        }
        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            SendEmailForSession();
        }

        private void PrintPDF(string filePath)
        {
            try
            {
                System.Diagnostics.Process printProcess = new System.Diagnostics.Process();
                printProcess.StartInfo.FileName = "explorer.exe";
                printProcess.StartInfo.Arguments = $"\"{filePath}\"";
                printProcess.StartInfo.CreateNoWindow = true;
                printProcess.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in PDF: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void Print_Click(object sender, RoutedEventArgs e)
        {
            string solutionRoot = GetPhotoHandler.FindSolutionRoot();
            string folderPath = System.IO.Path.Combine(solutionRoot, "PhotoBooth App", "Print");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = "Photo_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
            string pdfPath = System.IO.Path.Combine(folderPath, fileName);

            int totalPages = TakePhotoScreen.CurrentImages.Count;
            if (totalPages == 0)
            {
                MessageBox.Show("Không có ảnh nào để in!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProgressWindow progressWindow = new ProgressWindow();
            progressWindow.Show();

            try
            {
                await Task.Run(() =>
                {
                    using (PdfDocument document = new PdfDocument())
                    {
                        for (int i = 0; i < totalPages; i++)
                        {
                            string imagePath = TakePhotoScreen.CurrentImages[i];
                            BitmapImage bitmap = new BitmapImage();

                            if (imagePath.StartsWith("http"))
                            {
                                using (HttpClient client = new HttpClient())
                                {
                                    var imageBytes = client.GetByteArrayAsync(imagePath).Result;
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        bitmap.BeginInit();
                                        bitmap.StreamSource = ms;
                                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                        bitmap.EndInit();
                                        bitmap.Freeze();
                                    }
                                }
                            }
                            else
                            {
                                bitmap.BeginInit();
                                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                                bitmap.Freeze();
                            }

                            PdfPage page = document.AddPage();
                            page.Width = 595;
                            page.Height = 842;
                            XGraphics gfx = XGraphics.FromPdfPage(page);

                            double imgWidth = bitmap.PixelWidth;
                            double imgHeight = bitmap.PixelHeight;
                            double scale = Math.Min(page.Width / imgWidth, page.Height / imgHeight);
                            imgWidth *= scale;
                            imgHeight *= scale;

                            double x = (page.Width - imgWidth) / 2;
                            double y = (page.Height - imgHeight) / 2;

                            using (MemoryStream ms = new MemoryStream())
                            {
                                PngBitmapEncoder encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                                encoder.Save(ms);
                                ms.Position = 0;

                                XImage img = XImage.FromStream(ms);
                                gfx.DrawImage(img, x, y, imgWidth, imgHeight);
                            }

                            int progress = (i + 1) * 100 / totalPages;
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                progressWindow.AnimateProgress(progress);
                            });
                        }

                        document.Save(pdfPath);
                    }
                });

                progressWindow.Close();
                PrintPDF(pdfPath);
                MessageBox.Show($"Đã lưu và in PDF", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                var result = MessageBox.Show("Bạn có muốn tiếp tục không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    UserSession.Logout();
                    TakePhotoScreen.CurrentImages.Clear();
                    Login login = new Login();
                    login.Show();
                    Application.Current.MainWindow = login;
                    Window oldWindow = Window.GetWindow(this);
                    oldWindow?.Close();
                }
            }
            catch (Exception ex)
            {
                progressWindow.Close();
                MessageBox.Show($"Lỗi khi tạo/in PDF: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void SendEmailForSession()
        {
            string sessionCode = UserSession.SessionCode;
            if (string.IsNullOrEmpty(sessionCode))
            {
                MessageBox.Show("Không tìm thấy session!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync($"{baseUrl}/api/SendEmail/send/{sessionCode}", null);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Gửi email thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                        var result = MessageBox.Show("Bạn có muốn tiếp tục không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.No)
                        {
                            UserSession.Logout();
                            TakePhotoScreen.CurrentImages.Clear();
                            Login login = new Login();
                            login.Show();
                            Application.Current.MainWindow = login;
                            Window oldWindow = Window.GetWindow(this);
                            if (oldWindow != null)
                            {
                                oldWindow.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Gửi email thất bại: {response.ReasonPhrase}\nChi tiết: {responseContent}",
                                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SendZalo_Click(object sender, RoutedEventArgs e)
        {
            SendZaloWindow sendZaloWindow = new SendZaloWindow();
            sendZaloWindow.Owner = Application.Current.MainWindow;
            sendZaloWindow.ShowDialog();
        }
    }
}
