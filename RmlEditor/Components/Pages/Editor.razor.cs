using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using RmlEditor.Components.Custom;
using RmlEditor.Models;
using ThreeTwoSix.ReceiptRenderer;

namespace RmlEditor.Components.Pages
{
    public partial class Editor
    {
        [Parameter]
        public Guid Id { get; set; }


        public ReceiptRenderingService ReceiptRenderingService;

        private bool Validated = false;

        private MonacoEditor monacoEditor;

        private string initialCode = Constants.Template;

        public string CurrentCode { get; set; } = string.Empty;

        private bool OneBitImage = true;

        private bool isValidToRender = false;

        public bool isCodeValid;



        protected override async Task OnInitializedAsync()
        {
            ReceiptRenderingService = new ReceiptRenderingService();

            await base.OnInitializedAsync();
        }


        public async Task Validation()
        {
            string code = await monacoEditor.GetCodeAsync();
            
            bool isValid = await JS.InvokeAsync<bool>(identifier: "validateCustomXML", code);

            Validated = isValid;
            StateHasChanged();
        }


        

        //TODO Implement tha method to show and then Toggle Valid render ready with minimum template

        private string RenderedImageData = string.Empty;


        [JSInvokable]
        public async Task ShowError(string errorMessage)
        {
            Snackbar.Add($"{errorMessage}", Severity.Error);
        }


        public async Task RenderImageAsync(string UpdatedCode)
        {

            try
            {
                if (true)
                {
                    Console.WriteLine(CurrentCode);

                    CurrentCode = await monacoEditor.GetCodeAsync();

                    if (OneBitImage)
                    {

                        var buffer = ReceiptRenderingService.RenderOneBitPng(CurrentCode);

                        RenderedImageData = Convert.ToBase64String(buffer);
                    }
                    else
                    {
                        var buffer = ReceiptRenderingService.Render(CurrentCode, Constants.PNG);

                        RenderedImageData = Convert.ToBase64String(buffer);
                    }
                }
                Validated = false;
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{ex.Message}", Severity.Error);
            }
        }

    }

}
