using asyncDrive.Web.Services;
using asyncDrive.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace asyncDrive.Web.Controllers
{
    [Authorize]
    public class WebsiteController : Controller
    {
        private readonly WebsiteService _websiteService;

        public WebsiteController(WebsiteService websiteService)
        {
            _websiteService = websiteService;
        }

        public IActionResult CreateWebsite()
        {
            return View();
        }

        public IActionResult GetAllWebsite()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWebsites()
        {
            try
            {
                var accessToken = HttpContext.Session.GetString("AccessToken");
                if (string.IsNullOrEmpty(accessToken))
                {
                    return Unauthorized();
                }

                var websites = await _websiteService.GetAllWebsitesAsync(accessToken);
                return Json(websites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
