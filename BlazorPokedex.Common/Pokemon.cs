namespace BlazorPokedex.Common;

public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public PokemonType[] Types { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public int GenderRate { get; set; }
    public PokemonStat[] Stats { get; set; }
}