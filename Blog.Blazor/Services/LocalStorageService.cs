using Microsoft.JSInterop;

namespace Blog.Blazor.Services;

public class LocalStorageService
{
    private readonly IJSRuntime jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task SetItemAsync(string key, string value)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
    }

    public async Task<string> GetItemAsync(string key)
    {
        return await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    public async Task RemoveItemAsync(string key)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}