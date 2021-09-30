using BackEnd.Models.Models;
using FluentAssertions;
using Game.Shared.IServices;
using Game.Test.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Game.Test.Tests
{
    public class GameControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetCityNamesOfLoggedInUser()
        {
            // Arrange
            var client = this.CreateAuthenticatedClient();
            await InitDataBase();

            // Act
            var response = await client.GetAsync("api/View/CityNames");

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var data = response.Content.ReadAsStringAsync();

            await RollBack();

        }
    }
}
