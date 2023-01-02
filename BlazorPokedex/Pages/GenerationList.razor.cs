using BlazorPokedex.Common;
using BlazorPokedex.Logic;
using Microsoft.AspNetCore.Components;

namespace BlazorPokedex.Pages;

public partial class GenerationList
{
    [Inject]
    public PokeApi PokeApi { get; set; }

    private Generation[] Generations { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Generations = await PokeApi.GetGenerationsAsync();
    }
}