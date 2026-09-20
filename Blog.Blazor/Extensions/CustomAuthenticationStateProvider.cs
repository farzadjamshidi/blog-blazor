using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blog.Blazor.Extensions;

// BFF/Token Handler pattern (learning-notes/notes/50-bff-token-handler.md)
// — no token lives client-side anymore to inspect, so "am I logged in"
// means asking the server via bff/me (the HttpOnly session cookie is
// sent automatically by CookieCredentialsHandler). The ClaimsPrincipal
// built here is a placeholder either way — the pre-existing code this
// replaces did the same thing, never deriving real claims from the
// token either.
public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Called by Blazor itself whenever a component asks "is the current
    // user authenticated" (e.g. every <AuthorizeView>) — must not throw,
    // must resolve quickly since it can run on every render.
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = await ResolvePrincipalAsync();
        return new AuthenticationState(principal);
    }

    // Call this after anything that changes login state — a successful
    // login/register, or a logout — so every <AuthorizeView> in the app
    // re-renders with the new state. This is the direct replacement for
    // the old UpdateAuthenticationStateAsync(token): there's no token to
    // pass in anymore, so this just re-asks the server and broadcasts
    // whatever it says.
    public async Task RefreshAuthenticationStateAsync()
    {
        var principal = await ResolvePrincipalAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    // The actual "am I logged in" check. No token to inspect locally
    // anymore, so this asks blog-gateway's BFF endpoint instead —
    // CookieCredentialsHandler (on the AuthorizedClient pipeline) makes
    // sure the HttpOnly bff_session cookie actually gets sent along with
    // this call. 200 back means the gateway found a real token in its
    // session store for this cookie; 401 means it didn't (no session, or
    // an expired/evicted one).
    private async Task<ClaimsPrincipal> ResolvePrincipalAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("AuthorizedClient");
            var response = await client.GetAsync("bff/me");

            if (!response.IsSuccessStatusCode)
            {
                return _anonymous;
            }

            // A placeholder claim, not derived from the real JWT — same
            // fidelity as the code this replaced (it never decoded the
            // token either, just checked whether one existed). Real
            // claims (username, user profile id, ...) would need
            // blog-gateway to actually parse the stored JWT in bff/me
            // and return them; not done here, see learning-notes/notes/50-bff-token-handler.md.
            return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Name")
            }, "CookieAuth"));
        }
        catch (Exception)
        {
            // Network failure, gateway unreachable, etc. — fail closed
            // (treat as logged out) rather than let the exception bubble
            // up and break rendering.
            return _anonymous;
        }
    }
}
