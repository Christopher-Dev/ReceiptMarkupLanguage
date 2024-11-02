using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using RmlCommon.ServerModels;
using RmlCommon;
using System.Text.Json;

namespace RmlEditorWeb.Services
{
    public interface IRenderService
    {
        Task RequestRender(string renderRequestData);
        Task JoinGroup(string groupName);
        Task LeaveGroup(string groupName);
        Task StartConnectionAsync();
        Task StopConnectionAsync();
        event Action<CompletedRender> OnRenderResponseReceived;
    }



    public class RenderService : IRenderService
    {
        private readonly HubConnection _hubConnection;

        public event Action<CompletedRender> OnRenderResponseReceived;




        public RenderService(string hubUrl)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect() // Optional: enable automatic reconnection
                .Build();

            _hubConnection.Reconnecting += async (error) =>
            {
                Console.WriteLine("Reconnecting...");
                await Task.CompletedTask;
            };

            _hubConnection.Reconnected += async (connectionId) =>
            {
                Console.WriteLine("Reconnected!");
                await Task.CompletedTask;
            };

            // Handle connection events
            _hubConnection.On<string>("OnConnect", (connectionId) =>
            {
                Console.WriteLine($"Connected with ID: {connectionId}");
            });

            _hubConnection.On<string>("OnMemberJoin", (connectionId) =>
            {
                Console.WriteLine($"Member joined: {connectionId}");
            });

            _hubConnection.On<string>("OnMemberDisconnect", (connectionId) =>
            {
                Console.WriteLine($"Member disconnected: {connectionId}");
            });

            _hubConnection.On<string, string>("ReceiveRenderRequest", (connectionId, renderRequestData) =>
            {
                var result = JsonSerializer.Deserialize<SmartResponse<CompletedRender>>(renderRequestData);

                if (result != null && result.Data != null)
                {
                    OnRenderResponseReceived?.Invoke(result.Data);
                }

                Console.WriteLine($"Render request from {connectionId}: {renderRequestData}");
            });

            _hubConnection.On<string, string>("GroupMemberJoined", (connectionId, groupName) =>
            {
                Console.WriteLine($"Member {connectionId} joined group {groupName}");
            });

            _hubConnection.On<string, string>("GroupMemberLeft", (connectionId, groupName) =>
            {
                Console.WriteLine($"Member {connectionId} left group {groupName}");
            });
        }

        public async Task StartConnectionAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await _hubConnection.StartAsync();
                    Console.WriteLine("Connection started!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error starting connection: {ex.Message}");
                }
            }
        }
        public async Task StopConnectionAsync()
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.StopAsync();
                Console.WriteLine("Connection stopped.");
            }
        }
        public async Task RequestRender(string renderRequestData) => await _hubConnection.InvokeAsync("RequestRender", renderRequestData);
        public async Task JoinGroup(string groupName) => await _hubConnection.InvokeAsync("JoinGroup", groupName);
        public async Task LeaveGroup(string groupName) => await _hubConnection.InvokeAsync("LeaveGroup", groupName);
    }

}
