using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RmlServer
{

    public class RenderHub : Hub
    {
        // Method triggered when a client connects to the hub
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            // Notify the client that they have successfully connected
            await Clients.Client(connectionId).SendAsync("OnConnect", connectionId);

            // Notify all other clients about the new connection
            await Clients.Others.SendAsync("OnMemberJoin", connectionId);

            await base.OnConnectedAsync();
        }

        // Method triggered when a client disconnects from the hub
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;

            // Notify all other clients about the disconnection
            await Clients.Others.SendAsync("OnMemberDisconnect", connectionId);

            await base.OnDisconnectedAsync(exception);
        }



        // Method that allows a client to request a render
        public async Task RequestRender(string renderRequestData)
        {
            // Render the Image Here





            // Broadcast the render request to all clients in a specific group or globally
            await Clients.All.SendAsync("ReceiveRenderRequest", Context.ConnectionId, renderRequestData);
        }



        // Method to allow a client to join a specific group
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // Notify the group that the new member has joined
            await Clients.Group(groupName).SendAsync("GroupMemberJoined", Context.ConnectionId, groupName);
        }

        // Method to allow a client to leave a specific group
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            // Notify the group that the member has left
            await Clients.Group(groupName).SendAsync("GroupMemberLeft", Context.ConnectionId, groupName);
        }
    }

}
