using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using MudBlazor;
using System;
using ReceiptBuilder.Web.Models;
using Constants = RmlEditorWeb.Models.Constants;
using System.ComponentModel.DataAnnotations;
using RmlEditorWeb.Components;
using System.Diagnostics;
using System.Text;
using System.Net.Http.Json;


namespace RmlEditorWeb.Pages
{
    public partial class Editor
    {
        private MonacoEditor? monacoEditor;
        private string initialCode = Constants.Template;
        private string editorOutput = string.Empty;
        private bool isValid = true;
        private string codeContent = string.Empty;

        //public async Task RenderImageAsync(string UpdatedCode)
        //{

        //    try
        //    {
                
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}


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


        //public async Task Validation()
        //{
        //    string code = await monacoEditor.GetCodeAsync();

        //    bool isValid = await JS.InvokeAsync<bool>(identifier: "validateCustomXML", code);

        //    Validated = isValid;
        //    StateHasChanged();
        //}




        ////TODO Implement tha method to show and then Toggle Valid render ready with minimum template

        private string RenderedImageData = string.Empty;


        //[JSInvokable]
        //public async Task ShowError(string errorMessage)
        //{
        //    Snackbar.Add($"{errorMessage}", Severity.Error);
        //}



        public async Task RenderImageAsync()
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();

                // Retrieve code from the editor
                CurrentCode = await monacoEditor.GetCodeAsync();
                GetCodeTime = sw.ElapsedMilliseconds.ToString() + "ms";

                // Get the base URI from NavigationManager
                var baseAddress = NavigationManager.BaseUri;

                // Create an instance of HttpClient
                using HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) };

                // Wrap the XML markup in a JSON object
                var jsonObject = new { markup = CurrentCode };

                // Serialize the JSON object
                var content = JsonContent.Create(jsonObject);

                // Make the POST request
                HttpResponseMessage response = await client.PostAsync("/api/Renderer", content);

                // Read and store the response string
                if (response.IsSuccessStatusCode)
                {
                    RenderedImageData = await response.Content.ReadAsStringAsync();
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
