using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using PhotoBooth_App.Model;
using PhotoBooth_App.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
using static System.Collections.Specialized.BitVector32;

namespace PhotoBooth_App
{
    /// <summary>
    /// Interaction logic for DepositPage.xaml
    /// </summary>
    public partial class DepositPage : Page
    {
        private readonly ApiService _apiService;
        private TextBox _currentTextBox;
        private int typeSessionId;
        private decimal typeSessionPrice;
        public DepositPage(int id, decimal price)
        {
            InitializeComponent();
            _apiService = new ApiService();
            typeSessionId = id;
            typeSessionPrice = price;
            Keyboard quantityMenu = FindName("keyboard") as Keyboard;
            if (quantityMenu != null)
            {
                quantityMenu.QuantitySelected += Update;
                quantityMenu.DeleteClicked += Clear;
                quantityMenu.ChangeKeyboard += ShowTextKeyboard;
            }
            KeyboardText textMenu = FindName("textKeyboard") as KeyboardText;
            if (textMenu != null)
            {
                textMenu.TextSelected += UpdateText;
                textMenu.DeleteClicked += Clear;
                textMenu.ChangeKeyboard += ShowKeyboard;

            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _currentTextBox = sender as TextBox;
        }

        private void Update(int quantity)
        {
            if (_currentTextBox != null)
                _currentTextBox.Text += quantity.ToString();
        }

        private void UpdateText(string text)
        {
            if (_currentTextBox == email_txt || _currentTextBox == DiscountCode)
            {
                _currentTextBox.Text += text;
            }
        }

        private void Clear()
        {
            if (_currentTextBox != null)
                _currentTextBox.Text = "";
        }
        private void ShowTextKeyboard()
        {
            keyboard.Visibility = Visibility.Collapsed;
            textKeyboard.Visibility = Visibility.Visible;
        }
        private void ShowKeyboard()
        {
            textKeyboard.Visibility = Visibility.Collapsed;
            keyboard.Visibility = Visibility.Visible;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Payment.Visibility = Visibility.Visible;
        }
        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            testcreate();
        }


        private async void testcreate()
        {
            if (string.IsNullOrWhiteSpace(email_txt.Text))
            {
                MessageBox.Show("Vui lòng nhập email chính xác trước khi tạo order!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await TranformPhotoHub.Instance.StartConnectionAsync();
                var (isValid, message) = await TranformPhotoHub.Instance.RegisterSessionAsync("");

                var requestData = new
                {
                    Email = email_txt.Text,
                    Phone = phone_txt.Text,
                    TypeSessionId = typeSessionId,
                    PaymentMethodId = 3,
                    CouponCode = DiscountCode.Text,
                    ConnectionId = TranformPhotoHub.connectionId
                };

                HttpResponseMessage response = await _apiService.PostAsync("/api/Order/desktop", requestData);
                if (response != null && response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response data: " + responseData);

                    var orderResponse = JsonConvert.DeserializeObject<dynamic>(responseData);

                    if (orderResponse != null && !string.IsNullOrEmpty((string)orderResponse.paymentLink))
                    {
                        var code = (long)orderResponse.newOrder.code;
                        ShowPayment((string)orderResponse.paymentLink, code);
                    }
                    else
                    {
                        MessageBox.Show("Không nhận được URL thanh toán!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();

                    string errorMessage = !string.IsNullOrWhiteSpace(errorContent) ? errorContent : "Tạo đơn hàng thất bại!";

                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
        private void ShowPayment(string paymentUrl, long orderCode)
        {
            PaymentPage paymentWindow = new PaymentPage(paymentUrl, orderCode, OnPaymentSuccess);
            paymentWindow.Owner = Application.Current.MainWindow;
            paymentWindow.ShowDialog();
        }
        private void OnPaymentSuccess()
        {
            Login login = new Login(UserSession.SessionCode);
            login.Show();

            Application.Current.MainWindow = login;

            foreach (Window window in Application.Current.Windows)
            {
                if (window != login)
                {
                    window.Close();
                }
            }
            //  NavigationService.Navigate(new TakePhotoScreen());
        }

        public class OrderResponse
        {
            [JsonProperty("newOrder")]
            public OrderDetails NewOrder { get; set; }

            [JsonProperty("paymentLink")]
            public string PaymentLink { get; set; }
        }

        public class OrderDetails
        {
            [JsonProperty("code")]
            public long Code { get; set; }

        }

    }
}