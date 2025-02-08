using asyncDrive.Web.Models.DTO;
using asyncDrive.Web.Services.Interfaces;
using System.Net.Http;

namespace asyncDrive.Web.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly HttpClient _httpClient;
        public UserRoleService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }
        public Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<RoleDto>>("Roles/all-roles");
        }
      
        public Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserRolesAsync(string userId, List<string> roles)
        {
            throw new NotImplementedException();
        }

    }
}
