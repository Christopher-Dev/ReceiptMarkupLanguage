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

        public event Action<CompletedRender> OnRenderResultReceived;

        public event Action<CompletedRender> OnServerErrorReceived;

        public RenderService(NavigationManager navigationManager)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{navigationManager.BaseUri}renderHub")
                .WithAutomaticReconnect()
                .Build();

            // Set up listener for "ReceiveRenderResult" from the server
            _hubConnection.On<SmartResponse<CompletedRender>>("ReceiveRenderResult", (renderResult) =>
            {
                OnRenderResultReceived?.Invoke(renderResult.Data);
            });
            _hubConnection.On<SmartResponse<CompletedRender>>("ErrorMessageReceived", (renderResult) =>
            {
                OnRenderResultReceived?.Invoke(renderResult.Data);
            });
        }

        public async Task StartAsync()
        {
            if (_hubConnection.State != HubConnectionState.Connected)
            {
                await _hubConnection.StartAsync();
            }
        }
        public async Task RenderReceiptAsync(SmartRequest<RenderRequest> renderRequest)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.SendAsync("RenderReceipt", renderRequest);
            }
        }
        public async ValueTask DisposeAsync()
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.StopAsync();
            }
            await _hubConnection.DisposeAsync();
        }
    }


}
