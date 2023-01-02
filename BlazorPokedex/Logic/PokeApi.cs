using System.Net.Http.Json;
using BlazorPokedex.Common;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static BlazorPokedex.Pages.FetchData;

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

        return null;
    }
}
