using Microsoft.JSInterop;

namespace WebApp.Interop;

public interface IToastService
{
    Task ToastInfo(string message);
    Task ToastSuccess(string message);
    Task ToastError(string message);
}

public interface ICommonInterop
{
    Task<bool> Confirm(string message);
}

public class CommonInterop : IToastService, ICommonInterop
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public CommonInterop(IJSRuntime js)
    {
        moduleTask = new(() => js.InvokeAsync<IJSObjectReference>("import", "./js/common.js").AsTask());
    }

    public async Task ToastInfo(string message)
    {
        await Toast(message, "text-primary");
    }

    public async Task ToastSuccess(string message)
    {
        await Toast(message, "text-success");
    }

    public async Task ToastError(string message)
    {
        await Toast(message, "text-danger");
    }

    private async Task Toast(string text, string type = "", int closeAfterMs = 3000)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("toast", text, type, closeAfterMs);
    }

    public async Task<bool> Confirm(string message)
    {
        var module = await moduleTask.Value;
        var res = await module.InvokeAsync<bool>("confirmDialog", message);

        return res;
    }
}
