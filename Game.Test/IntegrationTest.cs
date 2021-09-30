using BackEnd.Models.Models;
using Game.Test.Helpers;
using Game.Test.WebApplicationFactory;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Game.Test
{
    public class IntegrationTest
    {
        private readonly TestingWebApplicationFactory<Server.Startup> _testingWebApplicationFactory;

        protected IntegrationTest()
        {
            _testingWebApplicationFactory = new TestingWebApplicationFactory<Server.Startup>();
        }

        protected HttpClient CreateClient()
        {
            return _testingWebApplicationFactory.CreateClient();
        }

        protected async Task InitDataBase()
        {
            var db = _testingWebApplicationFactory.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var buildingSeed = _testingWebApplicationFactory.ServiceProvider.GetRequiredService<BuildingUpgradeSeed>();

            await buildingSeed.InitializeDbForTestsAsync(db);
        }

        protected async Task RollBack()
        {
            var buildingSeed = _testingWebApplicationFactory.ServiceProvider.GetRequiredService<BuildingUpgradeSeed>();

            await buildingSeed.RollBack();
        }

        protected HttpClient CreateAuthenticatedClient()
        {
            var client = _testingWebApplicationFactory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication("Test")
                            .AddJwtBearer()
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "Test", options => { });
                    });

                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            return client;
        }

    }
}
