using PhotoBooth_App.Handler;
using PhotoBooth_App.Model;
using PhotoBooth_App.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoBooth_App
{
    public partial class StickerEditorPage : Page
    {
        private string imagePath;
        private List<Image> stickers = new List<Image>();
        private string stickerFolderPath;
        private Point _dragStartPoint;
        private readonly ApiService _apiService;

        public StickerEditorPage(string imagePath)
        {
            InitializeComponent();
            _apiService = new ApiService();
            this.imagePath = imagePath;
            string solutionRoot = GetPhotoHandler.FindSolutionRoot();
            stickerFolderPath = Path.Combine(solutionRoot, "PhotoBooth App", "Sticker");

            LoadImage();
            LoadStickers();
        }

        private void LoadImage()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            MainImage.Source = bitmap;
        }
        private async Task LoadStickers()
        {
            try
            {
                List<StickerItem> stickers = await _apiService.GetAsync<List<StickerItem>>("/api/Sticker/all");

                if (stickers != null && stickers.Count > 0)
                {
                    StickerList.Items.Clear();
                    foreach (var sticker in stickers)
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(sticker.StickerUrl, UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        Image stickerImage = new Image
                        {
                            Source = bitmap,
                            Width = 50,
                            Height = 50,
                            Cursor = Cursors.Hand,
                            Tag = sticker.Id
                        };

                        ListBoxItem item = new ListBoxItem { Content = stickerImage };
                        StickerList.Items.Add(item);
                    }
                }
                else
                {
                    MessageBox.Show("Không có sticker nào được tìm thấy.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy danh sách sticker: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StickerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StickerList.SelectedItem is ListBoxItem item && item.Content is Image sticker)
            {
                Image newSticker = new Image
                {
                    Source = sticker.Source,
                    Width = 100,
                    Height = 100,
                    Cursor = Cursors.Hand
                };
                newSticker.MouseLeftButtonDown += Sticker_MouseDown;
                PhotoCanvas.Children.Add(newSticker);
                stickers.Add(newSticker);

            }
        }
        private void Sticker_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image sticker)
            {
                if (e.ClickCount == 2)
                {
                    PhotoCanvas.Children.Remove(sticker);
                    stickers.Remove(sticker);
                }
                else if (e.ClickCount == 1)
                {
                    _dragStartPoint = e.GetPosition(PhotoCanvas);
                    sticker.MouseMove += Sticker_MouseMove;
                }
            }
        }
        private void Sticker_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Image sticker && e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject dragData = new DataObject("Sticker", sticker);
                DragDrop.DoDragDrop(sticker, dragData, DragDropEffects.Move);
            }
        }
        private void PhotoCanvas_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Sticker"))
            {
                e.Effects = DragDropEffects.Move;
            }
        }
        private void PhotoCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData("Sticker") is Image sticker)
            {
                Point dropPosition = e.GetPosition(PhotoCanvas);
                Canvas.SetLeft(sticker, dropPosition.X - (sticker.Width / 2));
                Canvas.SetTop(sticker, dropPosition.Y - (sticker.Height / 2));
                if (!PhotoCanvas.Children.Contains(sticker))
                {
                    PhotoCanvas.Children.Add(sticker);
                }
            }
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage originalBitmap = MainImage.Source as BitmapImage;
            if (originalBitmap == null)
            {
                MessageBox.Show("Không tìm thấy ảnh gốc!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int targetWidth = originalBitmap.PixelWidth;
            int targetHeight = originalBitmap.PixelHeight;

            EditingGrid.UpdateLayout();
            double gridWidth = EditingGrid.ActualWidth;
            double gridHeight = EditingGrid.ActualHeight;

            double scaleFactor = Math.Min(gridWidth / targetWidth, gridHeight / targetHeight);
            double imageDisplayWidth = targetWidth * scaleFactor;
            double imageDisplayHeight = targetHeight * scaleFactor;

            double offsetX = (gridWidth - imageDisplayWidth) / 2;
            double offsetY = (gridHeight - imageDisplayHeight) / 2;

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(targetWidth, targetHeight, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawImage(originalBitmap, new Rect(0, 0, targetWidth, targetHeight));

                double convertScale = targetWidth / imageDisplayWidth;

                foreach (var child in PhotoCanvas.Children)
                {
                    if (child is Image sticker && sticker.Source is BitmapSource stickerBitmap)
                    {
                        double stickerLeft = Canvas.GetLeft(sticker);
                        double stickerTop = Canvas.GetTop(sticker);

                        double relativeX = stickerLeft - offsetX;
                        double relativeY = stickerTop - offsetY;

                        double imageX = relativeX * convertScale;
                        double imageY = relativeY * convertScale;

                        double stickerWidth = sticker.Width * convertScale;
                        double stickerHeight = sticker.Height * convertScale;

                        dc.DrawImage(stickerBitmap, new Rect(imageX, imageY, stickerWidth, stickerHeight));
                    }
                }
            }
            renderBitmap.Render(dv);

            string savePath = Path.Combine(Path.GetDirectoryName(imagePath), "edited_" + Path.GetFileName(imagePath));
            using (FileStream fs = new FileStream(savePath, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(fs);
            }
            TakePhotoScreen.CurrentImages.Add(savePath);

            MessageBox.Show("Ảnh có sticker đã được lưu");
            NavigationService.Navigate(new CheckPhotoPage());
        }
    }
}
