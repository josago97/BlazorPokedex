using BlazorPokedex.Common;
using Microsoft.AspNetCore.Components;

namespace BlazorPokedex.BlazorCommon.Components;

public partial class PokemonTypeSlot
{
    private static readonly Dictionary<PokemonType, string> COLORS = new Dictionary<PokemonType, string>()
    {
        { PokemonType.Bug, "#92A212" }, { PokemonType.Dark, "#4F3F3D" },
        { PokemonType.Dragon, "#4F60E2" }, { PokemonType.Electric, "#FAC100" },
        { PokemonType.Fairy, "#EF71EF" }, { PokemonType.Fighting, "#FF8100" },
        { PokemonType.Fire, "#E72324" }, { PokemonType.Flying, "#82BAEF" },
        { PokemonType.Ghost, "#713F71" }, { PokemonType.Grass, "#3DA224" },
        { PokemonType.Ground, "#92501B" }, { PokemonType.Ice, "#3DD9FF" },
        { PokemonType.Normal, "#A0A2A0" }, { PokemonType.Poison, "#923FCC" },
        { PokemonType.Psychic, "#EF3F7A" }, { PokemonType.Rock, "#B0AB82" },
        { PokemonType.Steel, "#60A2B9" }, { PokemonType.Water, "#2481EF" },
    };

    [Parameter]
    public PokemonType Type { get; set; }

    public string Color => COLORS[Type];
}