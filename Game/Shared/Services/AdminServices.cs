using Game.Shared.Models.Request;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Game.Shared.Services
{
    public class AdminServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _adminController = "Admin";

        public AdminServices(HttpClient http) => _httpClient = http;

       
        public async Task<HttpResponseMessage> BanUser(UserBanRequest request) 
        {
            return await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + _adminController + "/Ban", request);
        }
       
        public async Task<HttpResponseMessage> CreateBuildingUpgradeCost(UpgradeCostCreationRequest request) 
        {
            return await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + _adminController + "/Create/UpgradeCost", request);
        }

       
        public async Task<HttpResponseMessage> ModifyBuildingUpgradeCost(UpgradeCostCreationRequest request) 
        {
            return await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + _adminController + "/Modify/UpgradeCost", request);
        }

        public async Task<HttpResponseMessage> ModerateCityName( CityNameModerationRequest request) 
        {
            return await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + _adminController + "/Moderate/Cityname", request);
        }


        public async Task<HttpResponseMessage> ModifyUnitCost(UnitCostModificationRequest request) 
        {
            return await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + _adminController + "/Modify/UnitCost", request);
        }
       
    }
}
