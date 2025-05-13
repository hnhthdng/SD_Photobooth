using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using PhotoBooth_App.Service;
using PhotoBooth_App.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace PhotoBooth_App
{
    /// <summary>
    /// Interaction logic for PaymentPage.xaml
    /// </summary>
    public partial class PaymentPage : Window
    {
        private string paymentUrl;
        private long orderCode;
        string baseUrl = AppConfig.GetApiUrl();
        private Action PaymentSuccess;

        public PaymentPage(string paymentUrl, long orderCode, Action paymentSuccess)
        {
            InitializeComponent();
            this.paymentUrl = paymentUrl;
            this.orderCode = orderCode;
            this.Loaded += PaymentPage_Loaded;
            PaymentSuccess = paymentSuccess;
        }

        private async void PaymentPage_Loaded(object sender, RoutedEventArgs e)
        {
            await webView.EnsureCoreWebView2Async();
            webView.Source = new Uri(paymentUrl);

            SignalR();
        }


        private void SignalR()
        {
            TranformPhotoHub.Instance.Connection.On<bool, string, long>("ConfirmPaymentPayOS", (cancel, status, orderCode) =>
            {
                Dispatcher.Invoke(async () =>
                {
                    if (cancel == true)
                    {
                        MessageBox.Show("Đã hủy thanh toán! Vui lòng order lại");
                        this.Close();
                    }
                    else if (status == "PAID" && cancel == false)
                    {
                        MessageBox.Show($"Thanh toán thành công! OrderCode: {orderCode}");
                        await CreateSessionAsync(orderCode);
                    }
                    else
                    {
                        MessageBox.Show($"Thanh toán thất bại. OrderCode: {orderCode}");
                    }
                    this.Close();
                });
            }
            );
        }



        private async Task CreateSessionAsync(long orderCode)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync($"{baseUrl}/api/Session/{orderCode}", null);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var session = JsonConvert.DeserializeObject<SessionResponse>(responseContent);
                        MessageBox.Show($"Session đã được tạo thành công! Session của bạn: {session.Code}",
                                        "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                        UserSession.UserRole = "Customer";
                        UserSession.SessionCode = session.Code;
                        PaymentSuccess?.Invoke();

                    }
                    else
                    {
                        MessageBox.Show($"Failed to create session: {response.ReasonPhrase}\nDetails: {responseContent}",
                                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi tạo session: {ex.Message}", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }


        public class SessionResponse
        {
            public string Code { get; set; }
        }
    }
}