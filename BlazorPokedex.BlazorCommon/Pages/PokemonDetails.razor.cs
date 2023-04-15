using BlazorPokedex.BlazorCommon.Logic;
using BlazorPokedex.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorPokedex.BlazorCommon.Pages;

public partial class PokemonDetails : IAsyncDisposable
{
    [Inject]
    public IPokeApi PokeApi { get; set; }
    [Inject]
    public IJSRuntime JsRuntime { get; set; }

    [Parameter]
    public int PokemonId { get; set; }

    private IJSObjectReference JSModule { get; set; }
    private Pokemon Pokemon { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        Pokemon = await PokeApi.GetPokemonAsync(PokemonId);  
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            string jsPath = "./" + Utils.GetStaticFileUrl("app.js");
            JSModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", jsPath);
            //await JSModule.InvokeVoidAsync("setFavicon", Pokemon.IconUrl);
        }
    }

    private string GetStatName(PokemonStat stat)
    {
        return stat.Type switch
        {
            PokemonStatType.HealPoint => "Heal",
            PokemonStatType.Attack => "Attack",
            PokemonStatType.Defense => "Defense",
            PokemonStatType.SpecialAttack => "Special attack",
            PokemonStatType.SpecialDefense => "Special defense",
            _ => "Speed"
        };
    }

    public async ValueTask DisposeAsync()
    {
        //await JSModule.InvokeVoidAsync("setFavicon", "favicon.ico");
    }
}