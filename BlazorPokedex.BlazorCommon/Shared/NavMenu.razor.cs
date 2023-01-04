using BlazorPokedex.BlazorCommon.Logic;
using BlazorPokedex.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPokedex.BlazorCommon.Shared;

public partial class NavMenu
{
    private bool _collapseNavMenu = true;

    [Inject]
    public IPokeApi PokeApi { get; set; }
    [Inject]
    public IJSRuntime JsRuntime { get; set; }

    private Generation[] Generations { get; set; }
    private IJSObjectReference JSModule { get; set; }
    private string NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Generations = await PokeApi.GetGenerationsAsync();

        JSModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./" + Utils.GetStaticFileUrl("app.js"));
    }

    private async Task ToggleNavMenuAsync()
    {
        if (await JSModule.InvokeAsync<bool>("isCollapsed"))
        {
            Console.WriteLine(_collapseNavMenu);
            _collapseNavMenu = !_collapseNavMenu;
            Console.WriteLine(_collapseNavMenu);
        }
    }
}