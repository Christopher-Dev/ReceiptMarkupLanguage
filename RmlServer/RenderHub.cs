using Microsoft.AspNetCore.SignalR;
using RmlCommon.ServerModels;
using RmlCommon;
using System.Threading.Tasks;
using ThreeTwoSix.ReceiptRenderer;
using ZXing;

namespace RmlServer
{

    public class RenderHub : Hub
    {

        private readonly ReceiptRenderingService _receiptRenderingService;

        public RenderHub()
        {
            _receiptRenderingService = new ReceiptRenderingService();
        }

        public override async Task OnConnectedAsync()
        {
            // You can retrieve the User ID from the Context if authentication is set up.
            // For simplicity, we'll use the ConnectionId as the UserId.
            string userId = Context.ConnectionId;

            string welcomeMessage = $"Welcome, {userId}!";

            // Send the welcome message to the connected client.
            await Clients.Caller.SendAsync("ReceiveMessage", welcomeMessage);

            await base.OnConnectedAsync();
        }
    }

}
