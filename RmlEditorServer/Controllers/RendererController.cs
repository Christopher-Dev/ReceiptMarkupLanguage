using Microsoft.AspNetCore.Mvc;
using ThreeTwoSix.ReceiptRenderer;


namespace RmlEditorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private ReceiptRenderingService _receiptRenderingService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _receiptRenderingService = new ReceiptRenderingService();
            _logger = logger;
        }


        public class RenderRequest
        {
            public Guid Id { get; set; }
            public string RawContents { get; set; }
        }

        public class RenderResponse
        {
            public Guid Id { get; set; }
            public byte[] Renderedbase64 { get; set; }
        }

        [HttpPost]
        public RenderResponse RenderImage([FromBody] RenderRequest request)
        {


            var results = _receiptRenderingService.RenderOneBitPng(request.RawContents);


            return new RenderResponse
            {
                Id = request.Id,
                Renderedbase64 = results
            };
        }

    }
}
