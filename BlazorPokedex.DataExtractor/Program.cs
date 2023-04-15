using System.Text.Json;
using System.Text.Json.Serialization;
using BlazorPokedex.Common;

namespace BlazorPokedex.DataExtractor;

class Program
{
    private const string FILENAME = "pokedex.json";
    private static readonly string[] TARGET_PROJECT_FOLDERS = new[]
    {
        "/BlazorPokedex.Maui/Resources/Raw/",
        "/BlazorPokedex.Wasm/wwwroot/resources/"
    };
    private static readonly Dictionary<string, PokemonType> TYPES_MAP = Enum.GetValues<PokemonType>()
            .ToDictionary(t => t.ToString().ToLower());

    private static readonly JsonSerializerOptions JSON_OPTIONS = new JsonSerializerOptions()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private static PokeApiNet.PokeApiClient _pokeApi = new PokeApiNet.PokeApiClient();


    static async Task Main(string[] args)
    {
        Console.WriteLine("Start extracting pokedex data...");

        IList<Generation> data = await GetGenerationsAsync();
        string json = JsonSerializer.Serialize(data, JSON_OPTIONS);

        File.WriteAllText(FILENAME, json);
        foreach (string folder in TARGET_PROJECT_FOLDERS)
        {
            if (TryGetProjectTargetFilePath(folder, out string path))
                File.WriteAllText(path, json);
        }

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
                ImageUrl = pokemon.Sprites.Other.OfficialArtwork.FrontDefault,
                Types = pokemon.Types.OrderBy(t => t.Slot).Select(ConvertToPokemonType).ToArray(),
                Height = pokemon.Height,
                Weight = pokemon.Weight,
                GenderRate = specie.GenderRate,
                Stats = GetStats(pokemon)
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

    private static PokemonStat[] GetStats(PokeApiNet.Pokemon pokemon)
    {
        return pokemon.Stats.Select(s => new PokemonStat()
        {
            Type = s.Stat.Name switch
            {
                "hp" => PokemonStatType.HealPoint,
                "attack" => PokemonStatType.Attack,
                "defense" => PokemonStatType.Defense,
                "special-attack" => PokemonStatType.SpecialAttack,
                "special-defense" => PokemonStatType.SpecialDefense,
                _ => PokemonStatType.Speed
            },
            Value = s.BaseStat
        }).ToArray();
    }

    private static async Task<object[]> GetEvolutionChainsAsync()
    {
        var evolutionChainPage = await _pokeApi.GetApiResourcePageAsync<PokeApiNet.EvolutionChain>(1000, 0);
        var allEvolutionChains = await _pokeApi.GetResourceAsync(evolutionChainPage.Results);

        return null;
    }

    private static bool TryGetProjectTargetFilePath(string targetProjectFolder, out string path)
    {
        bool success = false;
        path = null;

        DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        do
        {
            if (directory.GetFiles("*.sln").Any())
                success = true;
            else
                directory = directory.Parent;
        } while (!success && directory != null);


        if (success)
            path = directory.FullName + targetProjectFolder + FILENAME;

        return success;
    }
}