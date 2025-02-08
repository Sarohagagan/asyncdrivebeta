using asyncDriveAPI.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace asyncDriveAPI.DataAccess.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Check if roles already exist
            var roles = new[] { "SiteAdmin", "SuperAdmin", "User" };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    // Create a new role
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed the default user with "SiteAdmin" role if it doesn't exist
            var user = await userManager.FindByEmailAsync("gaganstunning@gmail.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "Gagan",
                    Email = "gaganstunning@gmail.com",
                    PasswordHash="gagan@01",
                    Password="gagan@01"
                };

                // Create the user with a password
                var result = await userManager.CreateAsync(user, "gagan@01");
                if (result.Succeeded)
                {
                    // Assign the "SiteAdmin" role to the user
                    await userManager.AddToRoleAsync(user, "SiteAdmin");
                }
                else
                {
                    // Handle user creation failure (e.g., log the error)
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    //var logger = serviceProvider.GetRequiredService<ILogger<SeedData>>();
                   // logger.LogError($"Error creating user: {errors}");
                }
            }
        }
    }

}
