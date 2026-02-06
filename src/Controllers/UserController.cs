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
            var user = await _userService.GetUserProfileAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<AddUserResponse>> CreateUser([FromBody] AddUserRequest request)
        {
            var result = await _userService.AddUserAsync(request);
            return Ok(result);
        }

    }
}