using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using BlazorPokedex;
using BlazorPokedex.Shared;
using BlazorPokedex.Logic;
using BlazorPokedex.Common;
using System.Drawing;

namespace BlazorPokedex.Pages;

public partial class PokemonDetails
{
    [Inject]
    public PokeApi PokeApi { get; set; }

    [Parameter]
    public int PokemonId { get; set; }

    private Pokemon Pokemon { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Pokemon = await PokeApi.GetPokemonAsync(PokemonId);
    }
}