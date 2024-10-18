using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
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


builder.Services.AddSingleton<IReceiptViewModel, ReceiptViewModel>();

builder.Services.AddScoped<HelperMethodService>();



builder.Services.AddMudServices();


await builder.Build().RunAsync();
