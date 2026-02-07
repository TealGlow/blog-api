using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace User.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserProfileService _userService;

        public UserController(IUserProfileService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserResponse>> GetUser(string id)
        {
            try
            {
                var user = await _userService.GetUserProfileAsync(id);
                return Ok(user);
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

        [HttpPost]
        public async Task<ActionResult<AddUserResponse>> CreateUser([FromBody] AddUserRequest request)
        {
            try
            {
                var result = await _userService.AddUserAsync(request);
                return Created("User", result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            request.Id = id;

            var userId = User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID claim is missing.");
            }
            if (userId != id)
            {
                return Unauthorized("User can only update their own account!");
            }
            try
            {
                await _userService.UpdateUserAsync(request);
                return NoContent();
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

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                var userId = User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID claim is missing.");
                }
                if (userId != id)
                {
                    return Unauthorized("User can only delete their own account!");
                }
                await _userService.DeleteUserAsync(id);
                return NoContent();
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

        // Login
        // [HttpPost("login")]
        // public async Task<ActionResult<AuthLoginResponse>> Login([FromBody] AuthLoginRequest request)
        // {
        //     try
        //     {
        //         var response = await _userService.LoginAsync(request);
        //         return Ok(response);

        //     }
        //     catch (ArgumentException ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        //     catch (KeyNotFoundException ex)
        //     {
        //         return NotFound(ex.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }

        // // logout
        // [HttpPost("logout")]
        // public async Task<ActionResult> Logout([FromBody] LogoutRequest request)
        // {
        //     try
        //     {
        //         await _userService.LogoutAsync(request);
        //         return NoContent();
        //     }
        //     catch (ArgumentException ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        //     catch (KeyNotFoundException ex)
        //     {
        //         return NotFound(ex.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }
    }
}