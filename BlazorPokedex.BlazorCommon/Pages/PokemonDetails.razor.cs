using BlazorPokedex.BlazorCommon.Logic;
using BlazorPokedex.Common;
using Microsoft.AspNetCore.Components;

namespace BlazorPokedex.BlazorCommon.Pages;

public partial class PokemonDetails
{
    [Inject]
    public IPokeApi PokeApi { get; set; }

    [Parameter]
    public int PokemonId { get; set; }

    private Pokemon Pokemon { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        Pokemon = await PokeApi.GetPokemonAsync(PokemonId);
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
}