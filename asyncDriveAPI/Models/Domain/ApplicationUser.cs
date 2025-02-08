using Microsoft.AspNetCore.Identity;

namespace asyncDriveAPI.Models.Domain
{
    public class ApplicationUser: IdentityUser
    {
        public string Password { get; set; }

        public ICollection<Website> Websites { get; set; }  // Navigation property for related Websites

    }
}
