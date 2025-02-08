using asyncDriveAPI.Models.Domain;
using asyncDriveAPI.Models.DTO;

namespace asyncDriveAPI.Services.Interfaces
{
    public interface IWebsiteRepository
    {
        Task<IEnumerable<WebsiteSummaryDto>> GetAllWebsitesAsync();
        Task<WebsiteDetailsDto> GetWebsiteByIdAsync(int id);
        Task<WebsiteDto> CreateWebsiteAsync(CreateWebsiteDto website);
        Task<WebsiteDto> UpdateWebsiteAsync(UpdateWebsiteDto website);
        Task<bool> DeleteWebsiteAsync(int id);
    }
}
