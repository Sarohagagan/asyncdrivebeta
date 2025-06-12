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
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(loginDto, _jsonOptions), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("auth/login", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Login failed: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenDto = JsonSerializer.Deserialize<TokenDto>(responseContent, _jsonOptions);

                if (tokenDto?.AccessToken == null || tokenDto?.RefreshToken == null)
                {
                    throw new Exception("Invalid token response from server");
                }

                return tokenDto;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"API request failed: {ex.Message}");
            }
        }

        public async Task<TokenDto> RefreshTokenAsync(string refreshToken, string userId)
        {
            try
            {
                var refreshRequest = new RefreshTokenRequestDto
                {
                    UserId = userId,
                    RefreshToken = refreshToken
                };

                var content = new StringContent(JsonSerializer.Serialize(refreshRequest, _jsonOptions), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("auth/refresh-token", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Token refresh failed: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenDto = JsonSerializer.Deserialize<TokenDto>(responseContent, _jsonOptions);

                if (tokenDto?.AccessToken == null || tokenDto?.RefreshToken == null)
                {
                    throw new Exception("Invalid token response from server");
                }

                return tokenDto;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"API request failed: {ex.Message}");
            }
        }
    }
}
