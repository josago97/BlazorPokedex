using BlazorPokedex.Common;

namespace BlazorPokedex.BlazorCommon.Logic;

public interface IPokeApi
{
	Task<Generation[]> GetGenerationsAsync();

	Task<Generation> GetGenerationAsync(int id);

	Task<Pokemon> GetPokemonAsync(int id);
}
