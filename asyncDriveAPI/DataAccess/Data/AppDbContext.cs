using asyncDriveAPI.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace asyncDriveAPI.DataAccess.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Website> Websites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure Website to have a one-to-many relationship with ApplicationUser
            modelBuilder.Entity<Website>()
                .HasOne(w => w.User)
                .WithMany(u => u.Websites)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Configure cascade delete if needed
        }
    }

}
