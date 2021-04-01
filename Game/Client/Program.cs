using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Game.Client.Helpers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Refit;
using System;
using System.Net.Http;
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
            builder.Services.AddBlazoredLocalStorage(config =>
                config.JsonSerializerOptions.WriteIndented = true);
            builder.Services.AddBlazoredSessionStorage(config =>
               config.JsonSerializerOptions.WriteIndented = true);

            //States helping with the data display without browser refresh
            builder.Services.AddSingleton<CityResourceState>();
            builder.Services.AddSingleton<CityIndexState>();
            builder.Services.AddSingleton<UnitsOfCityState>();
            builder.Services.AddScoped<CityDetailsState>();
            builder.Services.AddScoped<UpgradeQueueState>();

            builder.Services.AddRefitClient<Game.Shared.IServices.IViewService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/View"))
                 .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddRefitClient<Game.Shared.IServices.IGameService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/Game"))
                 .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddRefitClient<Game.Shared.IServices.IAdminService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/Admin"))
                 .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            await builder.Build().RunAsync();
        }
    }
}
