using asyncDrive.Web.Models.DTO;
using asyncDrive.Web.Services.Interfaces;
using System.Net.Http;

namespace asyncDrive.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<UserDto>>("Users/get-all");
        }

        public async Task<UserDetailDto> GetUserDetailAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<UserDetailDto>($"Users/user-detail{userId}");
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("Users", userDto);
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<UserDto> UpdateUserAsync(string userId, UpdateUserDto userDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"Users/{userId}", userDto);
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var response = await _httpClient.DeleteAsync($"{userId}");
            return response.IsSuccessStatusCode;
        }
    }
}
