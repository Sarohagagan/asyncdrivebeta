using asyncDriveAPI.Models.Domain;
using asyncDriveAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace asyncDriveAPI.Services
{
    public class TokenService:ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        // Generate Access Token and Refresh Token
        public (string AccessToken, string RefreshToken) GenerateTokens(ApplicationUser user)
        {
            var userRoles = _userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                 new Claim(ClaimTypes.Name, user.UserName),
             };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // JWT Token (Access Token)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(30);

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // Generate Refresh Token (simple GUID)
            var refreshToken = Guid.NewGuid().ToString();

            return (accessToken, refreshToken);
        }


        // Store the Refresh Token in AspNetUserTokens table
        public async Task StoreRefreshTokenAsync(ApplicationUser user, string refreshToken)
        {
            // Store the refresh token in AspNetUserTokens table
            var existingToken = await _userManager.GetAuthenticationTokenAsync(user, "Jwt", "RefreshToken");
            if (existingToken != null)
            {
                // If the refresh token already exists, remove it
                await _userManager.RemoveAuthenticationTokenAsync(user, "Jwt", "RefreshToken");
            }

            // Store the new refresh token
            await _userManager.SetAuthenticationTokenAsync(user, "Jwt", "RefreshToken", refreshToken);
        }

        // Validate the refresh token from AspNetUserTokens table
        public async Task<string> ValidateRefreshTokenAsync(ApplicationUser user, string refreshToken)
        {
            var storedToken = await _userManager.GetAuthenticationTokenAsync(user, "Jwt", "RefreshToken");
            if (storedToken == refreshToken)
            {
                return storedToken;
            }
            return null;
        }

        // Remove the refresh token when it's no longer valid
        public async Task RemoveRefreshTokenAsync(ApplicationUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "Jwt", "RefreshToken");
        }
    }

}