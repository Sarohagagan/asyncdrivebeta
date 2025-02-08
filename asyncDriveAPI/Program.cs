using asyncDriveAPI.DataAccess.Data;
using asyncDriveAPI.Models.Domain;
using asyncDriveAPI.Services.Interfaces;
using asyncDriveAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using asyncDriveAPI.Extensions;
using Microsoft.OpenApi.Models;
using asyncDrive.API.Services.Interfaces;
using asyncDrive.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//start custom code
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ASP.NET Core Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password requirements
    options.Password.RequireDigit = false;               // Require at least one digit
    options.Password.RequireLowercase = false;           // Require at least one lowercase letter
    options.Password.RequireUppercase = false;           // Require at least one uppercase letter
    options.Password.RequireNonAlphanumeric = false;     // Require at least one special character
    options.Password.RequiredLength = 5;                 // Minimum password length
    options.Password.RequiredUniqueChars = 0;           // Minimum number of unique characters

    // Lockout settings
    options.Lockout.AllowedForNewUsers = false;
    options.Lockout.MaxFailedAccessAttempts = 5;        // Max failed attempts before lockout
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Lockout duration

    // User settings
    options.User.RequireUniqueEmail = true;              // Ensure that the email address is unique

}).AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//end custom code

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//to implement token based authorization to Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "asyncDrive API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                },
                Scheme="Oauth2",
                Name=JwtBearerDefaults.AuthenticationScheme,
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
// Add Jwt Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Register dependencies
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IWebsiteRepository, WebsiteRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Call the seed method
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        // Seed roles
        await SeedData.Initialize(services, userManager, roleManager);
    }
    catch (Exception ex)
    {
        // Handle exceptions (e.g., log them)
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
