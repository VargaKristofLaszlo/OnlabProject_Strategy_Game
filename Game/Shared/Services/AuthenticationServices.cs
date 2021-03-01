using Shared.Models.Request;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class AuthenticationServices
    {
        private readonly HttpClient httpClient;

        public AuthenticationServices(HttpClient http) => httpClient = http;

        public async Task<HttpResponseMessage> RegisterAsync(UserCreationRequest request)
        {
            return await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}/Auth/Register", request);
        }

        public async Task<HttpResponseMessage> LoginAsync(UserLoginRequest request)
        {            
            return await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}/Auth/Login", request);
        }
    }
}
