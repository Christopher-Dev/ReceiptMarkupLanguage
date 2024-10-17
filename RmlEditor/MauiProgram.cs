using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using RmlEditor.Services;
using RmlEditor.ViewModel;

namespace RmlEditor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            #pragma warning disable CA1416 // Platform compatibility check
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();

            // Configure fonts and services specifically for Windows
            builder.UseMauiApp<App>();
            builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
            builder.Services.AddMauiBlazorWebView();

            #if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            #endif
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<IReceiptViewModel, ReceiptViewModel>();
            builder.Services.AddMudServices();

            return builder.Build();
            #pragma warning restore CA1416 // Platform compatibility check
        }
    }
}

