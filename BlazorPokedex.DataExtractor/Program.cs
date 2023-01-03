using System.Text.Json;
using BlazorPokedex.Common;

namespace BlazorPokedex.DataExtractor;

internal class Program
{
    private const string FILENAME = "pokedex.json";
    private static readonly Dictionary<string, PokemonType> TYPES_MAP = Enum.GetValues<PokemonType>()
            .ToDictionary(t => t.ToString().ToLower());

    private static PokeApiNet.PokeApiClient _pokeApi = new PokeApiNet.PokeApiClient();

    static async Task Main(string[] args)
    {
        Console.WriteLine("Start extracting pokedex data...");
        IList<Generation> data = await GetGenerationsAsync();
        string json = JsonSerializer.Serialize(data);
        File.WriteAllText(FILENAME, json);
        Console.WriteLine("Finish");
    }

    static async Task<IList<Generation>> GetGenerationsAsync()
    {
        List<Generation> result = new List<Generation>();

        var generationPage = await _pokeApi.GetNamedResourcePageAsync<PokeApiNet.Generation>();
        var allGenerations = await _pokeApi.GetResourceAsync(generationPage.Results);

        foreach (var generation in allGenerations)
        {
            result.Add(new Generation()
            {
                Id = generation.Id,
                Pokemons = await GetPokemonsAsync(generation),
            });
        }

        return result;
    }

    static async Task<Pokemon[]> GetPokemonsAsync(PokeApiNet.Generation generation)
    {
        var species = await _pokeApi.GetResourceAsync(generation.PokemonSpecies);
        var pokemons = await _pokeApi.GetResourceAsync(species.Select(s => s.Varieties.First().Pokemon));

        Pokemon[] result = new Pokemon[pokemons.Count];

        for (int i = 0; i < pokemons.Count; i++)
        {
            PokeApiNet.Pokemon pokemon = pokemons[i];
            PokeApiNet.PokemonSpecies specie = species[i];

            result[i] = new Pokemon()
            {
                Id = pokemon.Id,
                Name = specie.Names.First(n => n.Language.Name == "en").Name,
                ImageUrl = pokemon.Sprites.Other.DreamWorld.FrontDefault
                            ?? pokemon.Sprites.Other.OfficialArtwork.FrontDefault,
                Types = pokemon.Types.OrderBy(t => t.Slot).Select(ConvertToPokemonType).ToArray(),
                Height = pokemon.Height,
                Weight = pokemon.Weight,
                Stats = pokemon.Stats.Select(s => new PokemonStat()
                {
                    Name = s.Stat.Name,
                    Value = s.BaseStat
                }).ToArray()
            };
        };

        return result.OrderBy(p => p.Id).ToArray();
    }


    private static PokemonType ConvertToPokemonType(PokeApiNet.PokemonType type)
    {
        PokemonType result;

        if (!TYPES_MAP.TryGetValue(type.Type.Name, out result))
            throw new Exception($"Can't find type for {type.Type.Name}");

        return result;
    }
}