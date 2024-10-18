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
using ThreeTwoSix.ReceiptRenderer;
using System.Diagnostics;


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



        public ReceiptRenderingService ReceiptRenderingService;

        //private bool Validated = false;

        //private MonacoEditor monacoEditor;

        //private string initialCode = Constants.Template;

        public string CurrentCode { get; set; } = string.Empty;

        private bool OneBitImage = true;

        private bool isValidToRender = false;

        public bool isCodeValid;

        public string RenderTime { get; set; } = "UnRendered";

        protected override async Task OnInitializedAsync()
        {
            ReceiptRenderingService = new ReceiptRenderingService();

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

                    RenderTime = sw.ElapsedMilliseconds.ToString() + "ms";
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"{ex.Message}", Severity.Error);
            }
        }

    }

}
