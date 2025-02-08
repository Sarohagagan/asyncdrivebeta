using System.Threading.Tasks;
using asyncDriveAPI.Models.Domain;
using asyncDriveAPI.Models.DTO;
using asyncDriveAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace asyncDriveAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }

        public async Task<TokenDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return null; // Handle appropriately in your controller
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                return null; // Handle appropriately in your controller
            }

            // Generate Access Token and Refresh Token
            var (accessToken, refreshToken) = _tokenService.GenerateTokens(user);

            // Store Refresh Token in the AspNetUserTokens table
            await _tokenService.StoreRefreshTokenAsync(user, refreshToken);

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenDto> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var user = await _userManager.FindByIdAsync(refreshTokenRequestDto.UserId);

            if (user == null)
            {
                return null; // Handle appropriately in your controller
            }

            // Validate the refresh token
            var storedRefreshToken = await _tokenService.ValidateRefreshTokenAsync(user, refreshTokenRequestDto.RefreshToken);

            if (storedRefreshToken == null)
            {
                return null; // Handle appropriately in your controller
            }

            // Generate a new Access Token and Refresh Token
            var (newAccessToken, newRefreshToken) = _tokenService.GenerateTokens(user);

            // Remove the old refresh token and store the new one
            await _tokenService.RemoveRefreshTokenAsync(user);
            await _tokenService.StoreRefreshTokenAsync(user, newRefreshToken);

            return new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
