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
        public async Task RequestRender(SmartRequest<RenderRequest> request)
        {
            try
            {

                // Step 1: Query RAM DB for Body and prepare receipt models

                //DataContextModel dataModel = new DataContextModel();
                //ResourcesModel resourcesModel = new ResourcesModel();

                //ReceiptModel receipt = new ReceiptModel(dataModel, resourcesModel, request.Data.BodyContents);

                // Step 2: Render based on OneBitPng flag
                byte[] results;

                if (request.Data != null)
                {
                    if (request.Data.OneBitPng)
                    {
                        results = _receiptRenderingService.RenderOneBitPng(request.Data.BodyContents);
                    }
                    else
                    {
                        results = _receiptRenderingService.Render(request.Data.BodyContents, request.Data.MimeType);
                    }

                    // Step 3: Return the rendered image as a response
                    var result = new SmartResponse<CompletedRender>
                    {
                        Data = new CompletedRender(request.Data.Id, results)
                    };
                    var content = JsonContent.Create(result);
                    await Clients.Caller.SendAsync("ReceiveRenderRequest", Context.ConnectionId, content);


                }
                else
                {
                    var result = new SmartResponse<CompletedRender>
                    {
                        Error = "Request Data was null!!"
                    };
                    var content = JsonContent.Create(result);
                    await Clients.Caller.SendAsync("ReceiveRenderRequest", Context.ConnectionId, content);


                }
            }
            catch (Exception ex)
            {
                var result =  new SmartResponse<CompletedRender> 
                { 
                    Error = "An error occurred during rendering." 
                };
                
                var content = JsonContent.Create(result);
                await Clients.Caller.SendAsync("ReceiveRenderRequest", Context.ConnectionId, content);

            }
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
