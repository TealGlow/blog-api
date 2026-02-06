using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace User.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager _userManager;

        public UserController(UserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserResponse>> GetUser(string id)
        {
            var user = await _userManager.GetUsersAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<AddUserResponse>> CreateUser([FromBody] AddUserRequest request)
        {
            var result = await _userManager.AddUserAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
        }

    }
}