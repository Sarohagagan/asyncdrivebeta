using asyncDriveAPI.Models.DTO;
using asyncDriveAPI.Services;
using asyncDriveAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace asyncDriveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requires authentication for all actions by default
    public class WebsiteController : ControllerBase
    {
        private readonly IWebsiteRepository _websiteRepository;

        public WebsiteController(IWebsiteRepository websiteRepository)
        {
            _websiteRepository = websiteRepository;
        }

        // GET: api/website
        [HttpGet]
        [Authorize(Roles = "SiteAdmin,SuperAdmin")] // Only Admins and Users can view all websites
        public async Task<IActionResult> GetAllWebsites()
        {    
            var websites = await _websiteRepository.GetAllWebsitesAsync();  
            return Ok(websites);
        }

        // GET: api/website/{id}
        [HttpGet("{id}")]
       // [Authorize(Roles = "SiteAdmin,SuperAdmin")] // Only Admins and Users can view specific website details
        public async Task<IActionResult> GetWebsiteById(int id)
        {
            var website = await _websiteRepository.GetWebsiteByIdAsync(id);
            if (website == null)
            {
                return NotFound();
            }
            return Ok(website);
        }

        // POST: api/website
        [HttpPost]
        [Authorize(Roles = "SiteAdmin")] // Only Admins can create a new website
        public async Task<IActionResult> CreateWebsite([FromBody] CreateWebsiteDto websiteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdWebsite = await _websiteRepository.CreateWebsiteAsync(websiteDto);
            return CreatedAtAction(nameof(GetWebsiteById), new { id = createdWebsite.Id }, createdWebsite);
        }

        // PUT: api/website/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "SiteAdmin")] // Only Admins can update a website
        public async Task<IActionResult> UpdateWebsite(int id, [FromBody] UpdateWebsiteDto websiteDto)
        {
            if (!ModelState.IsValid || id != websiteDto.Id)
            {
                return BadRequest(ModelState);
            }

            var updatedWebsite = await _websiteRepository.UpdateWebsiteAsync(websiteDto);
            if (updatedWebsite == null)
            {
                return NotFound();
            }
            return Ok(updatedWebsite);
        }

        // DELETE: api/website/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "SiteAdmin")] // Only Admins can delete a website
        public async Task<IActionResult> DeleteWebsite(int id)
        {
            var result = await _websiteRepository.DeleteWebsiteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
