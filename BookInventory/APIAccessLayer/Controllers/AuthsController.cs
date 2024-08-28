using BookInventory.BusinessLogicAcessLayer.Models.AccountModels;
using BookInventory.BusinessLogicAcessLayer.Services.AuthService;
using BookInventory.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.APIAccessLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthsController(IAuthService authRepo)
        {
            _authService = authRepo;
        }

        [HttpGet("users")]
        [Authorize(Policy = "User_Read")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _authService.GetUsers();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegistrationModel request)
        {
            var response = await _authService.Register(
                new User { Username = request.Username }, request.Password
                );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginModel request)
        {
            var response = await _authService.Login(
              request.Username, request.Password
                );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordd(PasswordChangeModel resetModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.ChangePassword(resetModel.Username, resetModel.CurrentPassword, resetModel.NewPassword);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword([FromQuery] string username)
        {
            var response = await _authService.ForgotPassword(username);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ServiceResponse<string>>> ResetPassword([FromBody] PasswordResetModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.ResetPassword(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "User_Read")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "User_Edit")]
        public async Task<IActionResult> UpdateUser(UserUpdateModel user, int id)
        {
            var existingUser = await _authService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _authService.UpdateUser(user, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "User_Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _authService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _authService.DeleteUser(id);
            return NoContent();
        }

    }
}
