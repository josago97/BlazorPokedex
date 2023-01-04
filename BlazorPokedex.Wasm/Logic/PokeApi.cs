using BlazorPokedex.BlazorCommon.Logic;

namespace BlazorPokedex.Wasm.Logic;

public class PokeApi : BasePokeApi
{
	protected const string DATA_PATH = $"resources/{FILENAME}";

	private readonly IServiceProvider _serviceProvider;

	public PokeApi(IServiceProvider serviceProvider) : base()
	{
		_serviceProvider = serviceProvider;
	}

	protected override async Task<string> GetJsonDataAsync()
	{
		using var scope = _serviceProvider.CreateScope();
		HttpClient client = scope.ServiceProvider.GetRequiredService<HttpClient>();

		return await client.GetStringAsync(DATA_PATH);
	}
}
