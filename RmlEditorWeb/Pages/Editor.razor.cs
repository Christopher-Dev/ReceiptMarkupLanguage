using Microsoft.AspNetCore.Components;
using MudBlazor;
using Constants = RmlEditorWeb.Models.Constants;
using RmlEditorWeb.Components;
using System.Diagnostics;
using System.Net.Http.Json;
using RmlCommon;
using RmlCommon.ServerModels;


namespace RmlEditorWeb.Pages
{
    public partial class Editor
    {
        private MonacoEditor? monacoEditor;
        private string initialCode = Constants.Template;
        private string editorOutput = string.Empty;
        private bool isValid = true;
        private string codeContent = string.Empty;


        public async Task Validation()
        {
            string code = await monacoEditor.GetCodeAsync();

            StateHasChanged();
        }




        //private bool Validated = false;

        //private MonacoEditor monacoEditor;

        //private string initialCode = Constants.Template;

        public string CurrentCode { get; set; } = string.Empty;

        private bool OneBitImage = true;

        private bool isValidToRender = false;

        public bool isCodeValid;

        public string RenderTime { get; set; } = "UnRendered";

        public string GetCodeTime { get; set; } = "UnRendered";

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }




        ////TODO Implement tha method to show and then Toggle Valid render ready with minimum template

        private string RenderedImageData = string.Empty;




        public async Task RenderImageAsync()
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();

                // Retrieve code from the editor
                CurrentCode = await monacoEditor.GetCodeAsync();
                GetCodeTime = sw.ElapsedMilliseconds.ToString() + "ms";

                // Set the base URI to the specified address
                var baseAddress = "https://localhost:32785";

                // Create an instance of HttpClient with the specified base address
                using HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) };

                // Create the RenderRequest object
                var renderRequest = new RenderRequest
                {
                    Id = Guid.NewGuid(),         // Generate a unique ID or use a specific ID if needed
                    BodyContents = CurrentCode,    // Assuming CurrentCode holds the data for "RawContents"
                    MimeType = Constants.PNG,
                    OneBitPng = true,
                };

                var request = new SmartRequest<RenderRequest>()
                {
                    Data = renderRequest,
                };

                // Serialize the RenderRequest object
                var content = JsonContent.Create(request);

                // Make the POST request to the new endpoint
                HttpResponseMessage response = await client.PostAsync("/api/Render/RenderImage", content);

                // Handle the response
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response as RenderResponse
                    var renderResponse = await response.Content.ReadFromJsonAsync<SmartResponse<CompletedRender>>();

                    if (renderResponse.Data != null)
                    {
                        var stringResult = Convert.ToBase64String(renderResponse.Data.Receipt);

                        RenderedImageData = stringResult;
                    }
                    else
                    {
                        Snackbar.Add("Error: Unable to parse the response.", Severity.Error);
                    }
                }
                else
                {
                    Snackbar.Add($"Error: {response.ReasonPhrase}", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{ex.Message}", Severity.Error);
            }
        }
    }
}
