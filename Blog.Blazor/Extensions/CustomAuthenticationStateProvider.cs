using System.Security.Claims;
using Blog.Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blog.Blazor.Extensions;

public class CustomAuthenticationStateProvider: AuthenticationStateProvider
{
    private readonly LocalStorageService _localStorage;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(LocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync("authToken");
            if (token == null)
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Name")
                }, "JwtAuth"));
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch (Exception e)
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task UpdateAuthenticationStateAsync(string? token)
    {
        ClaimsPrincipal claimsPrincipal;
        if (token != null)
        {
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Name")
            }, "JwtAuth"));

            await _localStorage.SetItemAsync("authToken", token);
        }
        else
        {
            claimsPrincipal = _anonymous;
            await _localStorage.RemoveItemAsync("authToken");
        }
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task<string?> GetToken()
    {
        return await _localStorage.GetItemAsync("authToken");
    }
}