using asyncDrive.Web.Models.DTO;

namespace asyncDrive.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDetailDto> GetUserDetailAsync(string userId);
        Task<UserDto> CreateUserAsync(CreateUserDto userDto);
        Task<UserDto> UpdateUserAsync(string userId, UpdateUserDto userDto);
        Task<bool> DeleteUserAsync(string userId);
    }
}
