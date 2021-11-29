using AutoMapper;
using BackEnd.Models.Models;
using FluentAssertions;
using Game.Server;
using Game.Shared.Models.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Game.Test.IntegrationTests.Tests
{
    public class GetUnitCostByName : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GetUnitCostByName(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ViewController_GetUnitCostByNameFromDatabase()
        {
            using (var scope = _factory.Services.CreateScope())
            {

                // Arrange
                var client = _factory.CreateClient();

                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                var mapper = services.GetRequiredService<IMapper>();

                var units = dbContext.Units
                    .First(x => x.Name.Equals("Archer"));

                var expectedCost = mapper.Map<Shared.Models.Resources>(units.UnitCost);

                var expectedRepsonse = new Shared.Models.Resources()
                {
                    Wood = 80,
                    Stone = 30,
                    Silver = 60,
                    Population = 1
                };

                // Act
                var response = await client.GetFromJsonAsync<Shared.Models.Resources>($"/api/view/Units/Archer");

                // Assert
                response.Should().BeEquivalentTo(expectedRepsonse);
            }
        }
    }
}