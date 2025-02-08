using asyncDrive.API.Models.DTO;
using asyncDrive.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asyncDrive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public RolesController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet("all-roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _userRoleService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var success = await _userRoleService.CreateRoleAsync(roleName);
            if (success)
                return Ok(new { Message = "Role created successfully." });

            return BadRequest(new { Message = "Role already exists." });
        }

        [HttpPost("add-user-to-role")]
        public async Task<IActionResult> AddUserToRole([FromBody] UserRoleDto model)
        {
            var success = await _userRoleService.AddUserToRoleAsync(model.UserId, model.RoleName);
            if (success)
                return Ok(new { Message = "User added to role successfully." });

            return BadRequest(new { Message = "Failed to add user to role." });
        }
        [HttpPost("update-user-roles")]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesDto model,
    [FromServices] IUserRoleService userRoleService)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var success = await userRoleService.UpdateUserRolesAsync(model.UserId, model.Roles);
                if (success)
                    return Ok(new { Message = "User roles updated successfully." });

                return BadRequest(new { Message = "Failed to update user roles." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }

}
