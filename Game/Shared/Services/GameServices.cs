using Game.Shared.Models.Request;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Game.Shared.Services
{
    public class GameServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _gameController = "Game";

        public GameServices(HttpClient http) => _httpClient = http;


        //Refit is not using the enum properties correctly so this method is needed instead
        public async Task<HttpResponseMessage> AttackOtherCity(AttackRequest request)
        {
            return await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + _gameController + "/Attack", request);
        }

    }
}
