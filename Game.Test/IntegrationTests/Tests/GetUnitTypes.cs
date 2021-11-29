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
    public class GetUnitTypes : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GetUnitTypes(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ViewController_GetUnitTypesFromDatabase()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                // Arrange
                var client = _factory.CreateClient();

                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                var mapper = services.GetRequiredService<IMapper>();

                var units = dbContext.Units
                    .Select(type => mapper.Map<Shared.Models.Unit>(type))
                    .ToList();

                var expectedRepsonse = new CollectionResponse<Shared.Models.Unit>
                {
                    Records = units,
                    PagingInformations = new PagingInformations
                    {
                        PageNumber = 1,
                        PageSize = 20,
                        PagesCount = 1
                    }
                };

                // Act
                var response = await client.GetFromJsonAsync<CollectionResponse<Game.Shared.Models.Unit>>($"/api/view/Units?pageNumber=1&pageSize=20");

                // Assert
                response.Should().BeEquivalentTo(expectedRepsonse);
            }
        }
    }
}