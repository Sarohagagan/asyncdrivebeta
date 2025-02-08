using asyncDriveAPI.DataAccess.Data;
using asyncDriveAPI.Models.Domain;
using asyncDriveAPI.Models.DTO;
using asyncDriveAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace asyncDriveAPI.Services
{
    public class WebsiteRepository : IWebsiteRepository
    {
        private readonly AppDbContext _context;

        public WebsiteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WebsiteSummaryDto>> GetAllWebsitesAsync()
        {
          
            return await _context.Websites
                .Select(w => new WebsiteSummaryDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    DNS = w.DNS,
                    Status = w.Status
                })
                .ToListAsync();
        }

        public async Task<WebsiteDetailsDto> GetWebsiteByIdAsync(int id)
        {
            var website = await _context.Websites
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (website == null) return null;

            return new WebsiteDetailsDto
            {
                Id = website.Id,
                Title = website.Title,
                DNS = website.DNS,
                Description = website.Description,
                Status = website.Status,
                JsonData = website.JsonData,
                CreatedOn = website.CreatedOn,
                UpdatedOn = website.UpdatedOn,
                UserId = website.UserId,
                UserName = website.User.UserName,
                UserEmail = website.User.Email
            };
        }

        public async Task<WebsiteDto> CreateWebsiteAsync(CreateWebsiteDto createDto)
        {
            var website = new Website
            {
                Title = createDto.Title,
                DNS = createDto.DNS,
                Description = createDto.Description,
                Status = createDto.Status,
                JsonData = createDto.JsonData,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                UserId = createDto.UserId
            };

            _context.Websites.Add(website);
            await _context.SaveChangesAsync();

            return new WebsiteDto
            {
                Id = website.Id,
                Title = website.Title,
                DNS = website.DNS,
                Description = website.Description,
                Status = website.Status,
                JsonData = website.JsonData,
                CreatedOn = website.CreatedOn,
                UpdatedOn = website.UpdatedOn,
                UserId = website.UserId
            };
        }

        public async Task<WebsiteDto> UpdateWebsiteAsync(UpdateWebsiteDto updateDto)
        {
            var website = await _context.Websites.FindAsync(updateDto.Id);
            if (website == null) return null;

            website.Title = updateDto.Title;
            website.DNS = updateDto.DNS;
            website.Description = updateDto.Description;
            website.Status = updateDto.Status;
            website.JsonData = updateDto.JsonData;
            website.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new WebsiteDto
            {
                Id = website.Id,
                Title = website.Title,
                DNS = website.DNS,
                Description = website.Description,
                Status = website.Status,
                JsonData = website.JsonData,
                CreatedOn = website.CreatedOn,
                UpdatedOn = website.UpdatedOn,
                UserId = website.UserId
            };
        }

        public async Task<bool> DeleteWebsiteAsync(int id)
        {
            var website = await _context.Websites.FindAsync(id);
            if (website == null) return false;

            _context.Websites.Remove(website);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
