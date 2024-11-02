using Microsoft.AspNetCore.Components;
using MudBlazor;
using Constants = RmlEditorWeb.Models.Constants;
using RmlEditorWeb.Components;
using System.Diagnostics;
using System.Net.Http.Json;
using RmlCommon;
using RmlCommon.ServerModels;
using RmlEditorWeb.Components.Dialogs;
using System.Text.Json.Serialization;
using System.Text.Json;
using RmlEditorWeb.Services;


namespace RmlEditorWeb.Pages
{
    public partial class Editor
    {
        private MonacoEditor? monacoEditorRef;

        public async Task Validation()
        {
            string code = await monacoEditorRef.GetCodeAsync();

            StateHasChanged();
        }

        [Inject]
        private HttpClient Http { get; set; }

        [Inject]
        public IRenderService RenderService { get; set; }

        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            RenderService.OnRenderResponseReceived += HandleRenderResponse;

        }
        private void HandleRenderResponse(CompletedRender message)
        {
            RenderedImageData = Convert.ToBase64String(message.Receipt);
            InvokeAsync(StateHasChanged); // Update the UI
        }

        public void Dispose()
        {
            RenderService.OnRenderResponseReceived -= HandleRenderResponse;
        }
        private string InitialCode = "";

        public string CurrentCode { get; set; } = string.Empty;

        private bool OneBitImage = true;

        private bool isValidToRender = false;

        public bool isCodeValid;

        public string RenderTime { get; set; } = "UnRendered";

        public string GetCodeTime { get; set; } = "UnRendered";


        private async Task SelectEditorType()
        {
            // Define dialog parameters if needed (for passing data to the dialog)
            var parameters = new DialogParameters();

            // Define dialog options with CloseButton disabled and BackdropClick set to false
            var options = new DialogOptions
            {
                CloseButton = false, // Disable the close button in the header
                MaxWidth = MaxWidth.ExtraSmall,
                FullWidth = true,
                BackdropClick = false, // Prevent closing the dialog by clicking outside
                CloseOnEscapeKey = false // Prevent closing the dialog with the Escape key
            };

            // Show the dialog with parameters and options
            var dialog = DialogService.Show<ChooseYourEditor>("", parameters, options);

            // Wait for the dialog result
            var result = await dialog.Result;

            // Check the dialog result
            if (!result.Canceled)
            {
                isLoading = true;

                try
                {
                    // Determine action based on result data
                    if (result.Data is string && (string)result.Data == "Template")
                    {
                        // Path to the XML file relative to wwwroot
                        var xmlFilePath = "assets/ExampleTemplate.xml";

                        // Fetch the XML file
                        var results = await Http.GetStringAsync(xmlFilePath);

                        if (monacoEditorRef != null)
                        { 
                            await monacoEditorRef.SetCodeAsync(results);
                        
                            StateHasChanged();
                        }

                    }
                    else if (result.Data is string && (string)result.Data == "Blank")
                    {
                        // Set to empty string for Blank option
                        InitialCode = string.Empty;

                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    InitialCode = $"Error loading Template: {ex.Message}";
                }
                finally
                {
                    isLoading = false;
                }
            }
        }







        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(500); // Adjust delay as needed
                await SelectEditorType();
            }

            await base.OnAfterRenderAsync(firstRender);
        }




        ////TODO Implement tha method to show and then Toggle Valid render ready with minimum template

        private string RenderedImageData = string.Empty;


        private async Task HandleCodeChanged(string updatedCode)
        {
            // Handle the code change (e.g., save to database, update UI, etc.)
            Console.WriteLine("Code updated: " + updatedCode);
            // You can add additional logic here as needed
        }

        private async Task HandleValidation(bool isValid)
        {
            // Handle the validation status (e.g., display validation messages)
            Console.WriteLine("Is code valid: " + isValid);
            // You can add additional logic here as needed
        }



        public async Task RenderImageAsync()
        {
            try
            {
                if (monacoEditorRef != null)
                {
                    // Retrieve code from the editor
                    CurrentCode = await monacoEditorRef.GetCodeAsync();

                    // Create the RenderRequest object
                    var renderRequest = new RenderRequest
                    {
                        Id = Guid.NewGuid(),         // Generate a unique ID or use a specific ID if needed
                        BodyContents = CurrentCode,    // Assuming CurrentCode holds the data for "RawContents"
                        MimeType = Constants.PNG,
                        OneBitPng = true,
                    };

                    var request = new SmartRequest<RenderRequest>
                    {
                        Data = renderRequest,
                    };

                    // Serialize the request object to JSON
                    var jsonRequest = JsonSerializer.Serialize(request);

                    // Send the request to the SignalR hub method "RenderRequest"
                    await RenderingService.RequestRender(jsonRequest);

                    Snackbar.Add("Render request sent to the server.", Severity.Info);
                }
                else
                {
                    Snackbar.Add("Editor doesn't exist!", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{ex.Message}", Severity.Error);
            }
        }

    }
}
