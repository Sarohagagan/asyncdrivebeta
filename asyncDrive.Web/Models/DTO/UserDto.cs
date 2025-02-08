using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace asyncDrive.Web.Models.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<WebsiteDto> Websites { get; set; }
    }
    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class CreateUserDto
    {
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        // [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        public string SelectedRoleId { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }

    public class UserDetailDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
