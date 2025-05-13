using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BussinessObject.Models;
using EOSDigital.API;
using EOSDigital.SDK;
using System.Drawing;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
using PhotoBooth_App.Handler;
using ErrorHandlerPhoto = PhotoBooth_App.Handler.ErrorHandler;
using System.Windows.Input;
using System.Windows.Navigation;


namespace PhotoBooth_App
{
    public partial class TakePhotoScreen : Page
    {
        public static List<string> CurrentImages { get; private set; } = new List<string>();

        private CameraHandler cameraHandler;
        private UIHandler uiHandler;
        private DriveLens _currentFocusNearMode = DriveLens.Near1;
        private DriveLens _currentFocusFarMode = DriveLens.Far1;
        private readonly string historyFolderPath;

        public TakePhotoScreen()
        {
            InitializeComponent();
            uiHandler = new UIHandler(LiveViewImage, CaptureButton, FocusFarModeButton, FocusNearModeButton, NextButton, BlurLayer, CountdownCircle, CountdownTextBlock, ProgressArc);
            ErrorHandlerPhoto.Initialize(uiHandler);
            //       cameraHandler = new CameraHandler(uiHandler);


            if (UserSession.cameraHandler == null)
            {
                UserSession.cameraHandler = new CameraHandler(uiHandler);
            }
            else
            {
                UserSession.cameraHandler.SetUIHandler(uiHandler);
            }

            cameraHandler = UserSession.cameraHandler;
            cameraHandler.EnsureCameraReady();



            EOSDigital.API.ErrorHandler.SevereErrorHappened += ErrorHandlerPhoto.ErrorHandler_SevereErrorHappened;
            EOSDigital.API.ErrorHandler.NonSevereErrorHappened += ErrorHandlerPhoto.ErrorHandler_NonSevereErrorHappened;

            string solutionRoot = GetPhotoHandler.FindSolutionRoot();

            cameraHandler.DownloadedFilesUpdated += UpdateImagesList;

            //            UpdateImagesList(cameraHandler.GetDownloadedFiles());

            this.Loaded += TakePhotoScreen_Loaded;
        }

        private void TakePhotoScreen_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshImagesList();
        }

        public void RefreshImagesList()
        {
            UpdateImagesList(CurrentImages);
        }
        private void UpdateImagesList(List<string> newFiles)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CurrentImages = newFiles;
                List<BitmapImage> images = new List<BitmapImage>();
                foreach (var file in CurrentImages)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(file, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    images.Add(bitmap);
                }

                ImagesList.ItemsSource = images;
            });
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Image image && image.Source is BitmapImage bitmap)
            {
                NavigationService.Navigate(new PhotoSelectedPage(bitmap.UriSource.LocalPath));

            }
        }


        private async void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            await uiHandler.StartCountdown();
            cameraHandler.TakePhoto();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cameraHandler.Dispose();
        }

        private void Next_Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CheckPhotoPage());
        }

        private void FocusNearModeButton_Click(object sender, RoutedEventArgs e)
        {
            SetFocusNearMode(_currentFocusNearMode);
        }

        private void FocusFarModeButton_Click(object sender, RoutedEventArgs e)
        {
            SetFocusFarMode(_currentFocusFarMode);
        }

        public void SetFocusNearMode(DriveLens mode)
        {
            if (cameraHandler.MainCamera == null) return;
            CameraAttribute.FocusNearNextMap.TryGetValue(mode, out _currentFocusNearMode);
            cameraHandler.MainCamera.SendCommand(CameraCommand.DriveLensEvf, (int)_currentFocusNearMode);
            FocusNearModeButton.Content = CameraAttribute.FocusNearTextMap.GetValueOrDefault(_currentFocusNearMode);
        }

        public void SetFocusFarMode(DriveLens mode)
        {
            if (cameraHandler.MainCamera == null) return;
            CameraAttribute.FocusFarNextMap.TryGetValue(mode, out _currentFocusFarMode);
            cameraHandler.MainCamera.SendCommand(CameraCommand.DriveLensEvf, (int)_currentFocusFarMode);
            FocusFarModeButton.Content = CameraAttribute.FocusFarTextMap.GetValueOrDefault(_currentFocusFarMode);
        }

    }
}