using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blog.Blazor;
using Blog.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("ApiBaseUrl is not configured.");

// BFF/Token Handler pattern (learning-notes/notes/50-bff-token-handler.md)
// — this app never holds a JWT at all, only an HttpOnly session cookie,
// so every call goes through the same cookie-credentialed client. No
// more separate unnamed HttpClient for the pre-login calls.
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCookieCredentialsHandler(apiBaseUrl);
builder.Services.AddMudServices();

await builder.Build().RunAsync();