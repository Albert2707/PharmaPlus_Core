namespace Presentation.Controllers
{
    using Application.Interfaces;
    using Domain.Interface;
    using Microsoft.AspNetCore.Identity.Data;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.ValidateUser(request.Email, request.Password);
            if (user == null)
                return Unauthorized();

            var token = await _authService.GenerateJwtToken(user);
            return Ok(new { token });
        }
    }
}