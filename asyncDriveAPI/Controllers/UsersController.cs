using asyncDriveAPI.Models.DTO;
using asyncDriveAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace asyncDriveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userRepository.GetAllUsersAsync());
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }
        [HttpGet("user-detail{userId}")]
        public async Task<IActionResult> GetUserDetail(string userId)
        {
            var user = await _userRepository.GetUserDetailAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var user = await _userRepository.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto userDto)
        {
            var updatedUser = await _userRepository.UpdateUserAsync(userId, userDto);
            if (updatedUser == null) return NotFound();
            return Ok(updatedUser);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userRepository.DeleteUserAsync(userId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
