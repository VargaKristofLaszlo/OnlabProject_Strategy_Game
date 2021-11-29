using BackEnd.Models.Models;
using FluentAssertions;
using Game.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Game.Test.IntegrationTests.Tests
{
    public class GetCityNamesOfUserById : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GetCityNamesOfUserById(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ViewController_GetCityNamesOfUserByIdFromDatabase()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                var client = _factory.CreateClient();


                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<ApplicationDbContext>();

                var user = dbContext.Users.First(x => x.UserName.Equals("kristof"));

                // Act
                var response = await client.GetFromJsonAsync<IEnumerable<string>>($"/api/view/CityNames/{user.Id}");

                // Assert
                response.Count().Should().Be(1);
                response.First().Should().Be("first city");
            }
        }
    }
}