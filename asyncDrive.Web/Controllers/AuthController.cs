using asyncDrive.Web.Models;
using asyncDrive.Web.Services;
using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var token = await _authService.LoginAsync(model);
        if (token == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View();
        }
        HttpContext.Session.SetString("UserName", model.Username);
        HttpContext.Session.SetString("AccessToken", token.AccessToken);
        HttpContext.Session.SetString("RefreshToken", token.RefreshToken);
        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public IActionResult Logout()
    {
        ViewData["UserName"] = null;
        HttpContext.Session.Clear(); // Clear all session data
        return RedirectToAction("Login","Auth");
    }
}
