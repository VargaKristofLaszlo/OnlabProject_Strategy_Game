using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Game.Shared.Services
{
    public class ViewServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _viewController = "View";

        public ViewServices(HttpClient http) => _httpClient = http;

        public async Task<HttpResponseMessage> GetUserCredentials(int pageNumber, int pageSize)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _viewController + "/Credentials");
            builder.SetPagingQueryString(pageNumber, pageSize);
            return await _httpClient.GetAsync(builder.ToString());
        }      

        public async Task<HttpResponseMessage> GetBuildingUpgradeCost(string buildingName, int buildingStage)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _viewController + "/City");
            builder.SetQueryStringWithBuildingNameAndStage(buildingName, buildingStage);
            return await _httpClient.GetAsync(builder.ToString());
        }

        public async Task<HttpResponseMessage> GetCityNamesOfUser() 
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _viewController + "/CityNames");            
            return await _httpClient.GetAsync(builder.ToString());
        }

        public async Task<HttpResponseMessage> GetUnitTypes(int pageNumber, int pageSize) 
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _viewController + "/Units");
            builder.SetPagingQueryString(pageNumber, pageSize);
            return await _httpClient.GetAsync(builder.ToString());
        }

        public async Task<HttpResponseMessage> GetCityDetails(int cityIndex)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _viewController + "/City");
            builder.SetQueryStringWithCityIndex(cityIndex);
            return await _httpClient.GetAsync(builder.ToString());
        }

        public async Task<HttpResponseMessage> GetProducibleUnits(int cityIndex) 
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _viewController + "/Units/Producible");
            builder.SetQueryStringWithCityIndex(cityIndex);
            return await _httpClient.GetAsync(builder.ToString());
        }

        public async Task<HttpResponseMessage> GetResourcesOfTheCity(int cityIndex)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _viewController + "/City/Resources");
            builder.SetQueryStringWithCityIndex(cityIndex);
            return await _httpClient.GetAsync(builder.ToString());
        }

        public async Task<HttpResponseMessage> GetWarehouseCapacity(int cityIndex) 
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _viewController + "/Warehouse/Capacity");
            builder.SetQueryStringWithCityIndex(cityIndex);
            return await _httpClient.GetAsync(builder.ToString());
        }

        public async Task<HttpResponseMessage> GetUnitsOfCity(int cityIndex)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress + _viewController + "/UnitsOfCity");
            builder.SetQueryStringWithCityIndex(cityIndex);
            return await _httpClient.GetAsync(builder.ToString());
        }
    }
}
