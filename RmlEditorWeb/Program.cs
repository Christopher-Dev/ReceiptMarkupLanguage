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



builder.Services.AddScoped<RenderService>();

// Register other services
builder.Services.AddScoped<HelperMethodService>();
builder.Services.AddMudServices();

var host = builder.Build();

var renderService = host.Services.GetRequiredService<RenderService>();
await renderService.StartAsync();

await host.RunAsync();


