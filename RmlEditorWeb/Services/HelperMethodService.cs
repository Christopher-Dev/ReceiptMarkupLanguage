using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RmlEditorWeb.Services
{
    public class HelperMethodService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;

        // Constructor to initialize dependencies
        public HelperMethodService(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }

        // Static method to open URL in a new tab
        public async Task OpenInNewTab(string relativeOrAbsoluteUrl)
        {
            var absoluteUrl = _navigationManager.ToAbsoluteUri(relativeOrAbsoluteUrl).ToString();
            await _jsRuntime.InvokeVoidAsync("window.open", absoluteUrl, "_blank");
        }
    }
}
