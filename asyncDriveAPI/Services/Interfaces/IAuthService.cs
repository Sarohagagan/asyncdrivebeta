using asyncDriveAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace asyncDriveAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto model);
        Task<TokenDto> LoginAsync(LoginDto model);
        Task<TokenDto> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto);
    }

}
