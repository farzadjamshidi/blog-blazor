using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Blog.Blazor.Extensions;

// BFF/Token Handler pattern (learning-notes/notes/50-bff-token-handler.md)
// — this app never holds a JWT to attach as a Bearer header anymore.
// Instead, every request needs to actually send the browser's
// HttpOnly session cookie cross-origin, which fetch/XHR don't do by
// default. SetBrowserRequestCredentials(Include) is the WASM-specific
// equivalent of fetch's `credentials: 'include'`.
public class CookieCredentialsHandler : DelegatingHandler
{
    // Runs before every request this handler is attached to (both the
    // shared AuthorizedClient below, and the SignalR hub connection in
    // PostDetail.razor, which reuses this same handler). Without this,
    // the browser would silently omit the bff_session cookie on any
    // cross-origin request (blog-blazor's origin is a different port
    // than blog-gateway's) and every call would look logged-out.
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        return base.SendAsync(request, cancellationToken);
    }
}

public static class HttpClientExtensions
{
    // Replaces the old AddCustomAuthorizationHandler (which read a JWT
    // from localStorage and attached it as a Bearer header). There's
    // only one HttpClient registration in the whole app now — Login and
    // Register used to need a separate, unauthenticated client, but once
    // nothing attaches a bearer token client-side, that distinction
    // stops existing; every call just needs the cookie included.
    public static void AddCookieCredentialsHandler(this IServiceCollection services, string apiBaseUrl)
    {
        services.AddTransient<CookieCredentialsHandler>();

        // 9.4 — retry (exponential backoff + jitter), circuit breaker,
        // per-attempt and total-request timeouts, all with sensible
        // defaults from one call — runs at the DelegatingHandler layer,
        // so it works the same under Blazor WASM's Fetch-based transport.
        services.AddHttpClient("AuthorizedClient", client => client.BaseAddress = new Uri(apiBaseUrl))
            .AddHttpMessageHandler<CookieCredentialsHandler>()
            .AddStandardResilienceHandler();
    }
}
