using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using EOSDigital.API;
using EOSDigital.SDK;
using ErrorHandlerPhoto = PhotoBooth_App.Handler.ErrorHandler;

namespace PhotoBooth_App.Handler
{
    public class CameraHandler
    {
        private CanonAPI APIHandler;
        public Camera MainCamera { get; private set; }
        public event Action<Stream> LiveViewUpdated;
        public event Action<DownloadInfo> DownloadReady;
        public event Action<StateEventID, int> StateChanged;
        private UIHandler uiHandler;
        private List<string> DownloadedFiles = new List<string>();
        private string DownloadLogPath = Path.Combine(GetPhotoHandler.FindSolutionRoot(), "PhotoBooth App", "history", "download_log.txt");
        public event Action<List<string>> DownloadedFilesUpdated;

        private string SavePath = Path.Combine(GetPhotoHandler.FindSolutionRoot(), "PhotoBooth App", "history");

        public CameraHandler(UIHandler uiHandler)
        {
            InitializeCamera();
            this.uiHandler = uiHandler;
        }

        private void InitializeCamera()
        {
            try
            {
                APIHandler = new CanonAPI();
                APIHandler.CameraAdded += APIHandler_CameraAdded;
                RefreshCamera();
                }
            catch (DllNotFoundException)
            {
                ErrorHandler.ReportError("Canon DLLs not found!", true);
            }
            catch (Exception ex)
            {
                ErrorHandler.ReportError(ex.Message, true);
            }
        }

        private void APIHandler_CameraAdded(CanonAPI sender)
        {
            try
            {
                RefreshCamera();
            }
            catch (Exception ex)
            {
                ErrorHandler.ReportError(ex.Message, false);
            }
        }

        private void RefreshCamera()
        {
            var camList = APIHandler.GetCameraList();
            if (camList.Count > 0)
            {
                MainCamera = camList[0];
                OpenSession();
            }
            else
            {
                ErrorHandler.ReportError("No camera found.", true);
            }
        }

        private void OpenSession()
        {
            if (MainCamera != null)
            {
                MainCamera.OpenSession();
                MainCamera.LiveViewUpdated += MainCamera_LiveViewUpdated;
                MainCamera.StateChanged += MainCamera_StateChanged;
                MainCamera.DownloadReady += MainCamera_DownloadReady;
                MainCamera.SetCapacity(4096, int.MaxValue);
                MainCamera.SetSetting(PropertyID.SaveTo, (int)SaveTo.Host);
                StartLiveView();
            }
        }

        private void StartLiveView()
        {
            MainCamera.StartLiveView();
        }

        private void MainCamera_LiveViewUpdated(Camera sender, Stream img)
        {
            try
            {
                BitmapImage EvfImage = new BitmapImage();
                EvfImage.BeginInit();
                EvfImage.StreamSource = img;
                EvfImage.CacheOption = BitmapCacheOption.OnLoad;
                EvfImage.EndInit();
                EvfImage.Freeze();

                Application.Current.Dispatcher.InvokeAsync(() => uiHandler.SetImageAction(EvfImage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
            }
        }

        private void MainCamera_DownloadReady(Camera sender, DownloadInfo Info)
        {
            try
            {
                string newFileName = $"Photo_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                string fullPath = Path.Combine(SavePath, newFileName);
                sender.DownloadFile(Info, SavePath, fullPath);
                DownloadedFiles.Add(fullPath);

                File.AppendAllLines(DownloadLogPath, new[] { fullPath });

                DownloadedFilesUpdated?.Invoke(DownloadedFiles);
            }
            catch (Exception ex)
            {
                ErrorHandlerPhoto.ReportError(ex.Message, false);
            }
        }

        private void MainCamera_StateChanged(Camera sender, StateEventID eventID, int parameter)
        {
            try
            {
                if (eventID == StateEventID.Shutdown)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MainCamera.CloseSession();
                    });
                }
            }
            catch (Exception ex) { ErrorHandlerPhoto.ReportError(ex.Message, false); }
        }

        public void TakePhoto()
        {
            try
            {
                if (MainCamera?.SessionOpen == true && MainCamera.IsLiveViewOn)
                {
                    MainCamera.SetCapacity(4096, int.MaxValue);
                    MainCamera.TakePhotoShutter();
                }
            }
            catch (Exception ex)
            {
                ErrorHandlerPhoto.ReportError(ex.Message, false);
            }
        }

        //public List<string> GetDownloadedFiles()
        //{
        //    return File.Exists(DownloadLogPath) ? File.ReadAllLines(DownloadLogPath).ToList() : new List<string>();
        //}

        public void Dispose()
        {
            MainCamera?.Dispose();
            APIHandler?.Dispose();
        }
        public void EnsureCameraReady()
        {
            try
            {
                if (MainCamera == null)
                {
                    throw new Exception("MainCamera is null.");
                }

                if (!MainCamera.SessionOpen)
                {
                    OpenSession();
                }

                if (!MainCamera.IsLiveViewOn)
                {
                    StartLiveView();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EnsureCameraReady failed: {ex.Message}");
                throw; 
            }
        }


        public void SetUIHandler(UIHandler handler)
        {
            uiHandler = handler;
        }
    }
}