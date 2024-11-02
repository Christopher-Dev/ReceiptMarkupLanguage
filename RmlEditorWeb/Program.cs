using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RmlEditorWeb;
using RmlEditorWeb.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient with base address
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Bind app settings from configuration
var appSettings = new AppSettings();
builder.Configuration.GetSection("AppSettings").Bind(appSettings);
builder.Services.AddSingleton(appSettings);

// Configure RenderService with environment-specific URL
var renderHubUrl = builder.HostEnvironment.IsDevelopment()
    ? "http://localhost:32785/RenderHub" // Development URL
    : "https://rmltools.com/RenderHub";  // Production URL

builder.Services.AddSingleton<IRenderService>(sp => new RenderService(renderHubUrl));

// Register other services
builder.Services.AddScoped<HelperMethodService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
