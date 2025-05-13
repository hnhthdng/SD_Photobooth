using Microsoft.AspNetCore.SignalR.Client;
using PhotoBooth_App.Service;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoBooth_App.Services
{
    public class TranformPhotoHub
    {
        private static TranformPhotoHub _instance;
        public static TranformPhotoHub Instance => _instance ??= new TranformPhotoHub();
        public static string connectionId = "";

        private bool _isInitialized = false;
        private bool _handlerRegistered = false;

        public HubConnection Connection { get; private set; }
        string baseUrl = AppConfig.GetApiUrl();

        private TranformPhotoHub()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}/tranferPhotoHub")
                .WithAutomaticReconnect()
                .Build();
        }

        public async Task StartConnectionAsync()
        {
            if (_isInitialized) return;

            if (Connection == null)
            {
                Connection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}/tranferPhotoHub")
                .WithAutomaticReconnect()
                .Build();
            }

            if (Connection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await Connection.StartAsync();
                    Console.WriteLine("SignalR Connected!");

                    if (!_handlerRegistered)
                    {
                        Connection.On<string[]>("ReceiveProcessedPhoto", (data) =>
                        {
                            Console.WriteLine($"Received processed image: {string.Join(", ", data)}");
                        });

                        _handlerRegistered = true;
                    }

                    _isInitialized = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SignalR Connection Error: {ex.Message}");
                }
            }
        }
        public async Task StopConnectionAsync()
        {
            if (Connection.State == HubConnectionState.Connected)
            {
                await Connection.StopAsync();
                Console.WriteLine("SignalR Disconnected!");
            }
        }

        public async Task SendPhotoRequest(string imageUrl)
        {
            if (Connection.State == HubConnectionState.Connected)
            {
                await Connection.InvokeAsync("SendPhoto", new { ImageUrl = imageUrl });
                Console.WriteLine("Sent photo request to server.");
            }
            else
            {
                Console.WriteLine("Cannot send photo request. SignalR is not connected.");
            }
        }

        public async Task<(bool isValid, string message)> RegisterSessionAsync(string sessionCode)
        {
            try
            {
                var result = await Connection.InvokeAsync<SessionResponse>("GetConnectionId", sessionCode);

                connectionId = result.ConnectionId;

                if (result.Success)
                {
                    return (true, "Success");
                }

                return (false, result.Message);
            }
            catch (Exception ex)
            {
                return (false, $"Error registering session: {ex.Message}");
            }
        }

        public async Task<(bool isValid, string message)> RegisterAsync(string sessionCode)
        {
            try
            {
                var result = await Connection.InvokeAsync<SessionResponse>("GetConnectionId", sessionCode);

                return (result.Success, result.ConnectionId);
            }
            catch (Exception ex)
            {
                return (false, $"Error registering session: {ex.Message}");
            }
        }
    }

    public class SessionResponse
    {
        public bool Success { get; set; }
        public string ConnectionId { get; set; }
        public string Message { get; set; }
        public string SessionCode { get; set; }
        public string Status { get; set; }
    }
}