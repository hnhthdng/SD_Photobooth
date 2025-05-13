using Newtonsoft.Json;
using PhotoBooth_App.Service;
using PhotoBooth_App.Services;
using System.Net.Http;
using System.Windows;
using Wpf.Ui.Controls;
using static PhotoBooth_App.DepositPage;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace PhotoBooth_App
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : FluentWindow
    {
        private readonly ApiService _apiService;


        public Login()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }
        public Login(string sessionCode) : this()
        {
            AccessCodeTextBox.Text = sessionCode;
        }
        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.UserRole = "Guest";
            MainWindow1 mainWindow = new MainWindow1();
            mainWindow.Show();
            this.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string sessionCode = AccessCodeTextBox.Text.Trim();
            if (await IsValidSessionCodeAsync(sessionCode))
            {
                await TranformPhotoHub.Instance.StartConnectionAsync();

                var (isValid, message) = await TranformPhotoHub.Instance.RegisterSessionAsync(sessionCode);
                if (!isValid)
                {
                    await new MessageBox()
                    {
                        Title = "Thiết bị khác đang sử dụng",
                        Content = message,
                        CloseButtonText = "OK"
                    }.ShowDialogAsync();

                    await TranformPhotoHub.Instance.StopConnectionAsync();
                    return;
                }

                UserSession.UserRole = "Customer";
                UserSession.SessionCode = sessionCode;
                await new MessageBox()
                {
                    Title = "Thành công !",
                    Content = "Đăng nhập thành công!",
                    CloseButtonText = "OK"
                }.ShowDialogAsync();
                MainWindow1 mainWindow = new MainWindow1();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                var uiMessageBox = new MessageBox()
                {
                    Title = "Thất bại !",
                    Content = "Bạn đã nhập thất bại, vui lòng nhập lại !",
                    CloseButtonText = "OK",
                }
                .ShowDialogAsync();
            }
        }

        private async Task<bool> IsValidSessionCodeAsync(string sessionCode)
        {
            if (string.IsNullOrWhiteSpace(sessionCode))
            {
                await new MessageBox()
                {
                    Title = "Thất bại !",
                    Content = "Không được để trống !",
                    CloseButtonText = "OK"
                }.ShowDialogAsync();

                return false;
            }

            try
            {
                var requestData = new
                {
                    code = sessionCode,
                    boothId = AppConfig.GetBoothId()

                };

                HttpResponseMessage response = await _apiService.PostAsync("/api/Session/use-session", requestData);

                if (response != null && response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var sessionResponse = JsonConvert.DeserializeObject<SessionResponse>(responseData);

                    UserSession.OrderId = sessionResponse.OrderId;
                    UserSession.Expired = sessionResponse.Expired.DateTime;

                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    await new MessageBox()
                    {
                        Title = "Thất bại !",
                        Content = $"Lỗi máy chủ ({(int)response.StatusCode}): {errorContent}",
                        CloseButtonText = "OK"
                    }.ShowDialogAsync();
                }
            }
            catch (Exception)
            {
                await new MessageBox()
                {
                    Title = "Thất bại !",
                    Content = "Bạn đã nhập thất bại, vui lòng nhập lại !",
                    CloseButtonText = "OK"
                }.ShowDialogAsync();
            }

            return false;
        }



        public class SessionResponse
        {
            public long OrderId { get; set; }
            public DateTimeOffset Expired { get; set; }
        }
    }
}