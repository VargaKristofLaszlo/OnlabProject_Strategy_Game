using FluentAssertions;
using Game.Server;
using Game.Server.Controllers;
using Game.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Game.Test.IntegrationTests.Tests
{
    public class GetBuildingUpgradeCostTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GetBuildingUpgradeCostTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ViewController_GetBuildingUpgradeCostFromDatabase()
        {
            //  System.Diagnostics.Debugger.Launch();

            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetFromJsonAsync<BuildingUpgradeCost>("/api/view/Building/UpgradeCost?buildingName=Barrack&buildingStage=1");

            // Assert
            response.Wood.Should().Be(5);
            response.Stone.Should().Be(10);
            response.Silver.Should().Be(5);
            response.Population.Should().Be(1);
        }
    }
}
