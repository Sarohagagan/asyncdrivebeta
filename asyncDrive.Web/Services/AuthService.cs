using asyncDrive.Web.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace asyncDrive.Web.Services
{

    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("auth/login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TokenDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<TokenDto> RefreshTokenAsync(string refreshToken)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { RefreshToken = refreshToken }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("refresh-token", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TokenDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
