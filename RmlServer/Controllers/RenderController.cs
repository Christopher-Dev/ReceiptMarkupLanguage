using Microsoft.AspNetCore.Mvc;
using RmlCommon;
using RmlCommon.ServerModels;
using ThreeTwoSix.ReceiptRenderer;

namespace RmlServer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RenderController : ControllerBase
    {
        private readonly ReceiptRenderingService _receiptRenderingService;

        public RenderController()
        {
            _receiptRenderingService = new ReceiptRenderingService();
        }

        [HttpPost]
        public SmartResponse<CompletedRender> RenderImage([FromBody] SmartRequest<RenderRequest> request)
        {
            if (request?.Data == null)
            {
            }
            try
            {

                // Step 1: Query RAM DB for Body and prepare receipt models

                //DataContextModel dataModel = new DataContextModel();
                //ResourcesModel resourcesModel = new ResourcesModel();

                //ReceiptModel receipt = new ReceiptModel(dataModel, resourcesModel, request.Data.BodyContents);

                // Step 2: Render based on OneBitPng flag
                byte[] results;
                if (request.Data.OneBitPng)
                {
                    results = _receiptRenderingService.RenderOneBitPng(request.Data.BodyContents);
                }
                else
                {
                    results = _receiptRenderingService.Render(request.Data.BodyContents, request.Data.MimeType);
                }


                // Step 3: Return the rendered image as a response
                return new SmartResponse<CompletedRender>
                {
                    Data = new CompletedRender(request.Data.Id, results)
                };
            }
            catch (Exception ex)
            {
                return new SmartResponse<CompletedRender> { Error = "An error occurred during rendering." };
            }
        }
    }
}
