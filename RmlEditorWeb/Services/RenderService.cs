using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace RmlEditorWeb.Services
{
    public interface IRenderService
    {
        Task StartAsync();
        Task RequestRender(string renderRequestData);
        Task JoinGroup(string groupName);
        Task LeaveGroup(string groupName);
    }



    public class RenderService : IRenderService
    {
        private readonly HubConnection _hubConnection;

        public RenderService(NavigationManager navigationManager, string hubUrl)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

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

        public async Task StartAsync() => await _hubConnection.StartAsync();
        public async Task RequestRender(string renderRequestData) => await _hubConnection.InvokeAsync("RequestRender", renderRequestData);
        public async Task JoinGroup(string groupName) => await _hubConnection.InvokeAsync("JoinGroup", groupName);
        public async Task LeaveGroup(string groupName) => await _hubConnection.InvokeAsync("LeaveGroup", groupName);
    }

}
