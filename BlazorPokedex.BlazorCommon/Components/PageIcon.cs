using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPokedex.BlazorCommon.Components;

public class PageIcon : ComponentBase, IAsyncDisposable
{
    private const string JS_CHANGE_ICON_FUNCTION = "changeFavicon";

    private string _defaultIcon;
    private string _lastHRef;

    [Parameter]
    public string HRef { get; set; }
    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    private IJSObjectReference JSModule { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (_lastHRef != HRef)
        {
            if (JSModule == null)
            {
                string jsPath = "./" + Utils.GetStaticFileUrl("app.js");
                JSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", jsPath);
            }

            string lastIcon = await JSModule.InvokeAsync<string>(JS_CHANGE_ICON_FUNCTION, HRef);

            if (firstRender)
            {
                _defaultIcon = lastIcon;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (JSModule != null)
        {
            await JSModule.InvokeVoidAsync(JS_CHANGE_ICON_FUNCTION, _defaultIcon);
        }
    }
}
