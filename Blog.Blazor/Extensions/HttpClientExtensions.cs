
using System.Net.Http.Headers;
using Blog.Blazor.Services;

namespace Blog.Blazor.Extensions;

public class CustomAuthorizationMessageHandler : DelegatingHandler
{
    private readonly LocalStorageService _localStorage;

    public CustomAuthorizationMessageHandler(LocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync("authToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

public static class HttpClientExtensions
{
    public static void AddCustomAuthorizationHandler(this IServiceCollection services, string apiBaseUrl)
    {
        services.AddTransient<CustomAuthorizationMessageHandler>();

        // 9.4 — retry (exponential backoff + jitter), circuit breaker,
        // per-attempt and total-request timeouts, all with sensible
        // defaults from one call — runs at the DelegatingHandler layer,
        // so it works the same under Blazor WASM's Fetch-based transport.
        services.AddHttpClient("AuthorizedClient", client => client.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<CustomAuthorizationMessageHandler>()
            .AddStandardResilienceHandler();
    }
}