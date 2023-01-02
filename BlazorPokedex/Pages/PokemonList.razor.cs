using BlazorPokedex.Common;
using BlazorPokedex.Logic;
using Microsoft.AspNetCore.Components;

namespace BlazorPokedex.Pages;

public partial class PokemonList
{
    [Inject]
    public PokeApi PokeApi { get; set; }

    [Parameter]
    public int Generation { get; set; }

    private Pokemon[] Pokemons { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Pokemons = (await PokeApi.GetGenerationAsync(Generation)).Pokemons;
    }
}