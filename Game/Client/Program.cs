using Game.Client.Helpers;
using Game.Shared.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Game.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("Game.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/"))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Game.ServerAPI"));           
            builder.Services.AddApiAuthorization();
            builder.Services.AddMudServices();
            builder.Services.AddSingleton<PopulationState>();
            builder.Services.AddSingleton<CityResourceState>();

            await builder.Build().RunAsync();
        }
    }
}
