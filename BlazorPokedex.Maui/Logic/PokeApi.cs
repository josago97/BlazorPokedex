namespace BlazorPokedex.Maui.Logic;

public class PokeApi : BlazorCommon.Logic.BasePokeApi
{
	protected override async Task<string> GetJsonDataAsync()
	{
		using Stream stream = await FileSystem.OpenAppPackageFileAsync(FILENAME);
		using StreamReader reader = new StreamReader(stream);

		return await reader.ReadToEndAsync();
	}
}
