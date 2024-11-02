using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RmlEditorWeb;
using RmlEditorWeb.Services;
using RmlEditorWeb.ViewModel;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var appSettings = new AppSettings();
builder.Configuration.GetSection("AppSettings").Bind(appSettings);
builder.Services.AddSingleton(appSettings);

string hubUrl = builder.HostEnvironment.IsDevelopment()
    ? "http://localhost:32785/renderHub"
    : "https://hub.rmltools.com/renderHub";

builder.Services.AddSingleton<IReceiptViewModel, ReceiptViewModel>();
builder.Services.AddSingleton<IRenderService>(sp =>
    new RenderService(sp.GetRequiredService<NavigationManager>(), hubUrl));

builder.Services.AddScoped<HelperMethodService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
