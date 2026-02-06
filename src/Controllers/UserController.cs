using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace User.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        // private readonly IPasswordHasher<object> _passwordHasher;

        public UserController(IUserService userService)
        {
            _userService = userService;
            // _passwordHasher = passwordHasher;
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
    }
}