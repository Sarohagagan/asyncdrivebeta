using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace asyncDriveAPI.Models.DTO
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
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SelectedRoleId { get; set; }
    }

    public class UserDetailDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
