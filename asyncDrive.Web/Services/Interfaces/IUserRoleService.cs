using asyncDrive.Web.Models.DTO;

namespace asyncDrive.Web.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<RoleDto>> GetRolesAsync();
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleName);
        Task<bool> AddUserToRoleAsync(string userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userId);
        Task<bool> IsUserInRoleAsync(string userId, string roleName);
        Task<bool> UpdateUserRolesAsync(string userId, List<string> roles);
    }
}
