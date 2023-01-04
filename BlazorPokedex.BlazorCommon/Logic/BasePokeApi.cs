using System.Text.Json;
using BlazorPokedex.Common;

namespace BlazorPokedex.BlazorCommon.Logic;

public abstract class BasePokeApi : IPokeApi
{
	protected const string FILENAME = "pokedex.json";

	private Generation[] _data;

	private async Task EnsureInitAsync()
	{
		if (_data == null)
		{
			string json = await GetJsonDataAsync();

			_data = JsonSerializer.Deserialize<Generation[]>(json);
		}
		else
		{
			await Task.CompletedTask;
		}
	}

	protected abstract Task<string> GetJsonDataAsync();

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

			if (id <= pokemonsCount + generation.Pokemons.Length)
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