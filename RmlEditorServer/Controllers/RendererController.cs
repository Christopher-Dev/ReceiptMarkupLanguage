using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThreeTwoSix.ReceiptRenderer;

namespace RmlEditorServer.Controllers
{
    public class Constants
    {
        public static readonly string Template =
@"<Receipt>
    <DataContext>
        <Dictionary>

            <Pair Key=""TemplateValue"" Value=""123456"" />

        </Dictionary>
    </DataContext>
    <Resources>



    </Resources>
    <Body Width=""500px"" Background=""FFFFFF"" Margin=""5,5,5,5"">



    </Body>
</Receipt>";

        public static readonly string PNG = "image/png";

        public static readonly string JPEG = "image/jpeg";
    }


    [Route("api/[controller]")]
    [ApiController]
    public class RendererController : ControllerBase
    {
        public ReceiptRenderingService _receiptRenderingService;


        public RendererController() 
        {

            _receiptRenderingService = new ReceiptRenderingService();


        }




        [HttpPost]
        public async Task<IActionResult> Render([FromBody] RenderRequest request)
        {
            try
            {
                // Validate the markup
                if (string.IsNullOrWhiteSpace(request.Markup))
                {
                    return BadRequest("Markup cannot be empty.");
                }

                // Render the markup
                var buffer = _receiptRenderingService.RenderOneBitPng(request.Markup);

                // Convert to base64
                var base64Result = Convert.ToBase64String(buffer);

                return Ok(base64Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error rendering image: {ex.Message}");
                return StatusCode(500, "An error occurred while rendering the image.");
            }
        }


    }

    public class RenderRequest
    {
        public string Markup { get; set; }
    }
}
