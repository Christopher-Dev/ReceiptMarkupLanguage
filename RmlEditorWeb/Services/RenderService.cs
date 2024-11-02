using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using RmlCommon.ServerModels;
using RmlCommon;
using System.Text.Json;

namespace RmlEditorWeb.Services
{
    public class RenderService : IAsyncDisposable
    {
        private readonly HubConnection _hubConnection;

        public event Action<string> OnMessageReceived;

        public RenderService(NavigationManager navigationManager)
        {
            // Assuming the server and client are hosted on the same domain.
            // Adjust the URL if hosted separately.
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{navigationManager.BaseUri}renderHub")
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string>("ReceiveMessage", (message) =>
            {
                OnMessageReceived?.Invoke(message);
            });
        }

        public async Task StartAsync()
        {
            await _hubConnection.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _hubConnection.DisposeAsync();
        }
    }
}
