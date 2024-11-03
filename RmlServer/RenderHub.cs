using Microsoft.AspNetCore.SignalR;
using RmlCommon;
using ThreeTwoSix.ReceiptRenderer;
using System.Text.Json;
using RmlCommon.ServerModels;
using ZXing;

namespace RmlServer
{

    public class RenderHub : Hub
    {

        private ReceiptRenderingService _receiptRenderingService;

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


        public async Task RenderReceipt(SmartRequest<RenderRequest> renderRequest)
        {
            try
            {
                // Validate the receipt data
                if (renderRequest?.Data == null)
                {
                    await SendErrorResponse("Invalid receipt data.");
                    return;
                }

                // Render the receipt based on the request data
                byte[] renderedReceipt = renderRequest.Data.OneBitPng
                    ? _receiptRenderingService.RenderOneBitPng(renderRequest.Data.BodyContents)
                    : _receiptRenderingService.Render(renderRequest.Data.BodyContents, renderRequest.Data.MimeType);

                var renderResult = new SmartResponse<CompletedRender>
                {
                    Data = new CompletedRender(Guid.NewGuid(), renderedReceipt),
                    CreatedOn = DateTimeOffset.UtcNow
                };

                await Clients.Caller.SendAsync("ReceiveRenderResult", renderResult);
            }
            catch (Exception ex)
            {
                await SendErrorResponse(ex.ToString());
            }
        }

        private async Task SendErrorResponse(string message)
        {
            var errorResult = new SmartResponse<CompletedRender>
            {
                CreatedOn = DateTimeOffset.UtcNow,
                Error = message
            };
            await Clients.Caller.SendAsync("ErrorMessageReceived", errorResult);
        }



    }

}
