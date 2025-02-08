using asyncDrive.Web.Models;
using System.Net.Http.Headers;
using System.Text.Json;


namespace asyncDrive.Web.Services
{

    public class WebsiteService
    {
        private readonly HttpClient _httpClient;

        public WebsiteService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<WebsiteDto>> GetAllWebsitesAsync()
        {
            var response = await _httpClient.GetAsync("website");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<WebsiteDto>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WebsiteDto> GetWebsiteByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"website/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WebsiteDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
