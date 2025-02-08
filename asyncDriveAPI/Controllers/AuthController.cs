using asyncDriveAPI.DataAccess.Data;
using asyncDriveAPI.Models.Domain;
using asyncDriveAPI.Models.DTO;
using asyncDriveAPI.Services.Interfaces;
using asyncDriveAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace asyncDriveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var tokenDto = await _authService.LoginAsync(model);

            if (tokenDto == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(tokenDto);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var tokenDto = await _authService.RefreshToken(refreshTokenRequestDto);

            if (tokenDto == null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            return Ok(tokenDto);
        }
    }
}

