using asyncDrive.Web.Models.DTO;
using asyncDrive.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asyncDrive.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;

        public UsersController(IUserService userService, IUserRoleService userRoleService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users); // Return the view with the list of users
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> UserDetails(string userId)
        {
            var user = await _userService.GetUserDetailAsync(userId);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            var roles = await _userRoleService.GetRolesAsync();
            var roleSelectList = roles?.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.Name
            }).ToList() ?? new List<SelectListItem>();

            var model = new CreateUserDto
            {
                Roles = roleSelectList,
                SelectedRoleId = roleSelectList.FirstOrDefault()?.Value
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto model)
        {
            //if (!ModelState.IsValid)
            //{
                // Reload roles in case of validation failure
               // var roles = await _userRoleService.GetRolesAsync();
               // model.Roles = roles?.Select(r => new SelectListItem
                //{
                   // Value = r.Id.ToString(),
                   // Text = r.Name
               // }).ToList() ?? new List<SelectListItem>();

               // return View(model); // Return the view with the reloaded roles
            //}

            // Logic to create user with model.SelectedRoleId
            await _userService.CreateUserAsync(model);

            return RedirectToAction("AllUsers");
        }


        [HttpGet("{userId}/Edit")]
        public async Task<IActionResult> UpdateUser(string userId)
        {
            var user = await _userService.GetUserDetailAsync(userId);
            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
            };
            if (user == null) return NotFound();
            return View(updateUserDto);
        }

        [HttpPost("{userId}/Edit")]
        public async Task<IActionResult> UpdateUser(string userId, UpdateUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var updatedUser = await _userService.UpdateUserAsync(userId, userDto);
                if (updatedUser == null) return NotFound();
                return RedirectToAction("AllUsers");
            }
            return View(userDto);
        }

        [HttpGet("{userId}/Delete")]
        public async Task<IActionResult> DeleteUserConfirmation(string userId)
        {
            var user = await _userService.GetUserDetailAsync(userId);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost("{userId}/Delete")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (!result) return NotFound();
            return RedirectToAction("AllUsers");
        }

      

    }
}
