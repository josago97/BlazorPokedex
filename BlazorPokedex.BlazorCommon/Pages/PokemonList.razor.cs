using BlazorPokedex.BlazorCommon.Logic;
using BlazorPokedex.Common;
using Microsoft.AspNetCore.Components;

namespace BlazorPokedex.BlazorCommon.Pages;

public partial class PokemonList
{
    [Inject]
    public IPokeApi PokeApi { get; set; }

    [Parameter]
    public int Generation { get; set; }

    private Pokemon[] Pokemons { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        Pokemons = (await PokeApi.GetGenerationAsync(Generation)).Pokemons;
    }
}