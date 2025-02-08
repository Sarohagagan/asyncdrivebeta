using asyncDriveAPI.Models.Domain;

namespace asyncDriveAPI.Services.Interfaces
{
    public interface ITokenService
    {
        (string AccessToken, string RefreshToken) GenerateTokens(ApplicationUser user);
        Task StoreRefreshTokenAsync(ApplicationUser user, string refreshToken);
        Task<string> ValidateRefreshTokenAsync(ApplicationUser user, string refreshToken);
        Task RemoveRefreshTokenAsync(ApplicationUser user);
    }
}
