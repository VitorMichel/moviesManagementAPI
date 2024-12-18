using Microsoft.AspNetCore.Mvc;
using MovieManagementApi.Services;

namespace MovieManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _tokenService;

        public AuthController(JwtTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Hardcoded example
            if (model.Username == "admin" && model.Password == "123")
            {
                var token = _tokenService.GenerateToken(model.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password.");
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
