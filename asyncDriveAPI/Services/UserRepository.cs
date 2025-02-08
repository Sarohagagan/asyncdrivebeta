using asyncDriveAPI.DataAccess.Data;
using asyncDriveAPI.Models.Domain;
using asyncDriveAPI.Models.DTO;
using asyncDriveAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    // Get all users
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Websites)
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Websites = u.Websites.Select(w => new WebsiteDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    DNS = w.DNS,
                    Description = w.Description,
                    Status = w.Status,
                    JsonData = w.JsonData,
                    CreatedOn = w.CreatedOn,
                    UpdatedOn = w.UpdatedOn,
                    UserId = w.UserId
                }).ToList()
            })
            .ToListAsync();
    }

    // Get user by Id
    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        var user = await _context.Users
            .Include(u => u.Websites)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Websites = user.Websites.Select(w => new WebsiteDto
            {
                Id = w.Id,
                Title = w.Title,
                DNS = w.DNS,
                Description = w.Description,
                Status = w.Status,
                JsonData = w.JsonData,
                CreatedOn = w.CreatedOn,
                UpdatedOn = w.UpdatedOn,
                UserId = w.UserId,
            }).ToList()
        };
    }
    public async Task<UserDetailDto> GetUserDetailAsync(string userId)
    {
        var user = await _context.Users
          .Include(u => u.Websites)
          .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return null;

        return new UserDetailDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Password = user.Password
        };
    }
    // Create user
    public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
    {
        try
        {
            PasswordHasher<IdentityUser> _passwordHasher = new PasswordHasher<IdentityUser>();

            // Create new user
            var user = new ApplicationUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = _passwordHasher.HashPassword(new IdentityUser(), userDto.Password), // Hash the password before saving
                NormalizedUserName = userDto.UserName.ToUpper(),
                NormalizedEmail = userDto.Email.ToUpper(),
                Password = userDto.Password
            };

            // Add user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Assign the user to the role
            var userRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = userDto.SelectedRoleId // Ensure SelectedRoleId is passed in userDto
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            // Return user details
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }
        catch (Exception ex)
        {
            // Log the exception for debugging purposes
            Console.WriteLine($"An error occurred: {ex.Message}");

            // Optionally, rethrow the exception or handle it accordingly
            throw new Exception("An error occurred while creating the user.", ex);
        }
    }


    // Update user
    public async Task<UserDto> UpdateUserAsync(string userId, UpdateUserDto userDto)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null) return null;
        PasswordHasher<IdentityUser> _passwordHasher = new PasswordHasher<IdentityUser>();
        user.Email = userDto.Email ?? user.Email;
        user.UserName = userDto.UserName ?? user.UserName;
        user.NormalizedEmail = userDto.Email.ToUpper();
        user.NormalizedUserName = userDto.UserName.ToUpper();
        user.Password=userDto.Password;
        user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
    }

    // Delete user
    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
