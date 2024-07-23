using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blog.Blazor;
using Blog.Blazor.Extensions;
using Blog.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7163/") });
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddCustomAuthorizationHandler();

await builder.Build().RunAsync();