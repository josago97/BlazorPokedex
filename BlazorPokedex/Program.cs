using BlazorPokedex.Logic;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorPokedex
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            //builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<PokeApi>();

            await builder.Build().RunAsync();
        }
    }
}