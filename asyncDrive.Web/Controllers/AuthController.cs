using System.IdentityModel.Tokens.Jwt;
using asyncDrive.Web.Models;
using asyncDrive.Web.Services;
using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
{
    private readonly AuthService _authService;
    private const string UserIdKey = "UserId";
    private const string UserNameKey = "UserName";
    private const string AccessTokenKey = "AccessToken";
    private const string RefreshTokenKey = "RefreshToken";

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString(AccessTokenKey) != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model)
    {
        try
        {
            var token = await _authService.LoginAsync(model);
            
            // Extract user ID from the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.AccessToken);
            var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid" || claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Unable to extract user ID from token");
            }

            // Store tokens and user info in session
            HttpContext.Session.SetString(UserIdKey, userId);
            HttpContext.Session.SetString(UserNameKey, model.Username);
            HttpContext.Session.SetString(AccessTokenKey, token.AccessToken);
            HttpContext.Session.SetString(RefreshTokenKey, token.RefreshToken);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var userId = HttpContext.Session.GetString(UserIdKey);
            var refreshToken = HttpContext.Session.GetString(RefreshTokenKey);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(refreshToken))
            {
                return RedirectToAction(nameof(Login));
            }

            var newTokens = await _authService.RefreshTokenAsync(refreshToken, userId);

            // Update session with new tokens
            HttpContext.Session.SetString(AccessTokenKey, newTokens.AccessToken);
            HttpContext.Session.SetString(RefreshTokenKey, newTokens.RefreshToken);

            return Ok(new { message = "Token refreshed successfully" });
        }
        catch (Exception)
        {
            // If token refresh fails, redirect to login
            return RedirectToAction(nameof(Login));
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
