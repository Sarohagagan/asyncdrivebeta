using asyncDrive.API.Models.DTO;
using asyncDrive.API.Services.Interfaces;
using asyncDriveAPI.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace asyncDrive.API.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {            
            return await _roleManager.Roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name }).ToListAsync();
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return false;

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _roleManager.RoleExistsAsync(roleName))
                return false;

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _roleManager.RoleExistsAsync(roleName))
                return false;

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<RoleDto>();

            var roleNames = await _userManager.GetRolesAsync(user); // List<string>
            var roles = new List<RoleDto>();

            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    roles.Add(new RoleDto { Id = role.Id, Name = role.Name });
                }
            }

            return roles;
        }


        public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null && await _userManager.IsInRoleAsync(user, roleName);
        }
        public async Task<bool> UpdateUserRolesAsync(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return false;

            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    var addResult = await _userManager.AddToRoleAsync(user, role);
                    if (!addResult.Succeeded)
                        return false;
                }
                else
                {
                    throw new Exception($"Role '{role}' does not exist.");
                }
            }

            return true;
        }

    }

}
