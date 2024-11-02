using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RmlEditorWeb;
using RmlEditorWeb.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var appSettings = new AppSettings();
builder.Configuration.GetSection("AppSettings").Bind(appSettings);
builder.Services.AddSingleton(appSettings);


// Configure RenderService with environment-specific URL
var renderHubUrl = builder.HostEnvironment.IsDevelopment()
    ? "http://localhost:32785/RenderHub" // Development URL
    : "https://rmltools.com/RenderHub";  // Production URL

builder.Services.AddSingleton<IRenderService>(sp => new RenderService(renderHubUrl));

builder.Services.AddScoped<HelperMethodService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
