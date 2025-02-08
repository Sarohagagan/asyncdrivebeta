using asyncDriveAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace asyncDriveAPI.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<UserDetailDto> GetUserDetailAsync(string userId);
        Task<UserDto> CreateUserAsync(CreateUserDto userDto);
        Task<UserDto> UpdateUserAsync(string userId, UpdateUserDto userDto);
        Task<bool> DeleteUserAsync(string userId);
    }
}
