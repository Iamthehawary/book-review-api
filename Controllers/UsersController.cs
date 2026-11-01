using BookReviewApi.DTOs;
using BookReviewApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) => _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);

            if (!result.Succeeded)
                return Conflict(new { message = result.Error });

            return StatusCode(StatusCodes.Status201Created, result.User);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _userService.LoginAsync(dto);

            if (token == null)
                return Unauthorized(new { message = "Incorrect username or password" });

            return Ok(new { token });
        }
    }
}