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

        public async Task<HttpResponseMessage> UpgradeBuilding(int cityIndex, string buildingName, int newStage)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _gameController + "/" + buildingName + "/Upgrade");
            builder.SetQueryStringForBuildingStageModification(cityIndex, newStage);
            return await _httpClient.PatchAsync(builder.ToString(), null);
        }

        public async Task<HttpResponseMessage> DowngradeBuilding(int cityIndex, string buildingName, int newStage) 
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _gameController + "/" + buildingName + "/Downgrade");
            builder.SetQueryStringForBuildingStageModification(cityIndex, newStage);
            return await _httpClient.PatchAsync(builder.ToString(), null);
        }

        public async Task<HttpResponseMessage> ProduceUnits(UnitProductionRequest request)
        {
            return await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + _gameController + "/ProduceUnit", request);
        }

        public async Task<HttpResponseMessage> SendResourcesToOtherPlayer(SendResourceToOtherPlayerRequest request) 
        {
            return await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + _gameController + "/Resources/Send", request);
        }

        public async Task<HttpResponseMessage> AttackOtherCity(AttackRequest request)
        {
            return await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + _gameController + "/Attack", request);
        }
    }
}
