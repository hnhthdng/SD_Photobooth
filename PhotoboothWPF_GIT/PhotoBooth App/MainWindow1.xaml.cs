using PhotoBooth_App.Handler;
using PhotoBooth_App.Service;
using PhotoBooth_App.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace PhotoBooth_App
{
    /// <summary>
    /// Interaction logic for MainWindow1.xaml
    /// </summary>
    public partial class MainWindow1 : FluentWindow
    {
        private CountdownTimer countdownTimer;
        public MainWindow1()
        {
            InitializeComponent();
            MainFrame.Navigated += MainFrame_Navigated;
            if (UserSession.UserRole == "Guest")
            {
                MainFrame.NavigationService.Navigate(new QuantitySelectionPage());
            }
            else
            {
                MainFrame.NavigationService.Navigate(new SelectFramePage());
            }
            LanguageHandler.SwitchLanguage("vi");
            var expireTime = DateTime.Parse(UserSession.Expired.ToString());
            if (expireTime > DateTime.MinValue && expireTime > DateTime.UtcNow)
            {
                countdownTimer = new CountdownTimer();
                countdownTimer.OnTimeChanged += UpdateCountdownText;
                countdownTimer.OnTimerFinished += TimerFinished;

                TimeSpan difference = expireTime - DateTime.UtcNow;
                countdownTimer.Start(difference);
            }
        }
        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Content is Page currentPage)
            {
                if (currentPage.Title != null)
                    TitleControl.TitleKey = currentPage.Title;

                bool showBack = true;

                if (currentPage is IBackButton backVisibilityPage)
                {
                    showBack = backVisibilityPage.ShowBackButton;
                }

                BackButtonControl.SetVisibility(showBack);
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string language = selectedItem.Tag.ToString();
                LanguageHandler.SwitchLanguage(language);
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var currentPage = MainFrame.Content as Page;

            if (currentPage is SelectFramePage || currentPage is QuantitySelectionPage)
            {
                UserSession.Logout();

                var loginWindow = new Login();
                loginWindow.Show();

                Window.GetWindow(this)?.Close();
            }
            else if (currentPage is CheckPhotoPage)
            {
                DateTime now = DateTime.UtcNow;
                DateTime expired = DateTime.Parse(UserSession.Expired.ToString());
                if (expired > now)
                {
                    MainFrame.GoBack();
                }
                else
                {
                    var uiMessageBox = new MessageBox()
                    {
                        Title = "Time Up!",
                        Content = "Thời gian đã hết, không thể quay lại phần chụp ảnh!",
                        CloseButtonText = "Ok"

                    };
                    uiMessageBox.ShowDialogAsync();
                }
            }
            else if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }


        private void UpdateCountdownText(string time, SolidColorBrush color)
        {
            Dispatcher.Invoke(() =>
            {
                DurationTimeLeftTextBlock.Text = time;
                DurationTimeLeftTextBlock.Foreground = color;
            });
        }

        private void TimerFinished()
        {
            Dispatcher.Invoke(async () =>
            {
                var uiMessageBox = new MessageBox()
                {
                    Title = "Time Up!",
                    Content = "Thời gian đã hết, bạn vui lòng chọn ảnh để in hoặc gửi ảnh qua email hoặc zalo bạn nhé !",
                    CloseButtonText = "Tạo và in ảnh"

                };
                var result = await uiMessageBox.ShowDialogAsync();
                if (result == Wpf.Ui.Controls.MessageBoxResult.None || result == Wpf.Ui.Controls.MessageBoxResult.Primary)
                {

                    MainFrame.NavigationService.Navigate(new CheckPhotoPage());
                }


                DurationTimeLeftTextBlock.Foreground = new SolidColorBrush(Colors.Red); 
            });
        }
    }
}
