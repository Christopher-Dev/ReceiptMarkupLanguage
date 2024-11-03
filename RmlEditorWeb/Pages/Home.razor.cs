
using Microsoft.AspNetCore.Components;
using RmlEditorWeb.Services;

namespace RmlEditorWeb.Pages
{
    public partial class Home
    {
        private string welcomeMessage;


        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }

        private void HandleMessageReceived(string message)
        {
            welcomeMessage = message;

            Snackbar.Add($"{message}");

            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
        }
    }
}
