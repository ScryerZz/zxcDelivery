using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace zxcBLAZOR.Services
{
    public class NotificationService
    {
        private readonly HubConnection _hubConnection;

        public event Action<string> OnNotificationReceived;

        public NotificationService()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/notificationHub")
                .Build();

            _hubConnection.On<string>("ReceiveNotification", (message) =>
            {
                OnNotificationReceived?.Invoke(message);
            });

            StartConnectionAsync();
        }

        private async void StartConnectionAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения к SignalR: {ex.Message}");
            }
        }

        public async Task SendNotification(string message)
        {
            await _hubConnection.SendAsync("SendNotification", message);
        }
    }
}