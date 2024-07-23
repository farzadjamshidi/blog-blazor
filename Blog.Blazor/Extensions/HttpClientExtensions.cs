
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
    public static void AddCustomAuthorizationHandler(this IServiceCollection services)
    {
        services.AddTransient<CustomAuthorizationMessageHandler>();

        services.AddHttpClient("AuthorizedClient")
            .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
    }
}