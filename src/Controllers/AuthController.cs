using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Login
        [HttpPost("login")]
        public async Task<ActionResult<AuthLoginResponse>> Login([FromBody] AuthLoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // logout
        [HttpPost("logout")]
        public async Task<ActionResult> Logout([FromBody] AuthLogoutRequest request)
        {
            try
            {
                var result = await _authService.LogoutAsync(request);
                if (result)
                {
                    return Ok(new { Success = true, Message = "Logout successful." });
                }
                else
                {
                    return BadRequest(new { Success = false, Message = "Logout failed." });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}