using System.Net.Http.Json;
using BlazorPokedex.Common;

namespace BlazorPokedex.Logic;

public class PokeApi
{
    private readonly IServiceProvider _serviceProvider;
    private Generation[] _data;

    public PokeApi(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private async Task EnsureInitAsync()
    {
        if (_data == null)
        {
            using var scope = _serviceProvider.CreateScope();
            HttpClient client = scope.ServiceProvider.GetRequiredService<HttpClient>();
            _data = await client.GetFromJsonAsync<Generation[]>("resources/pokedex.json");
        }
        else
        {
            await Task.CompletedTask;
        }
    }

    public async Task<Generation[]> GetGenerationsAsync()
    {
        await EnsureInitAsync();

        return _data;
    }

    public async Task<Generation> GetGenerationAsync(int id)
    {
        await EnsureInitAsync();

        return _data.FirstOrDefault(g => g.Id == id);
    }

    public async Task<Pokemon> GetPokemonAsync(int id)
    {
        await EnsureInitAsync();

        Pokemon result = null;
        int pokemonsCount = 0;
        int count = 0;

        while (result == null && count < _data.Length)
        {
            Generation generation = _data[count];

            if (id <= generation.Pokemons.Length)
            {
                result = generation.Pokemons[id - pokemonsCount - 1];
            }
            else
            {
                pokemonsCount += generation.Pokemons.Length;
                count++;
            }
        }

        return result;
    }
}
