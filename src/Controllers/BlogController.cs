
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<ActionResult<GetAllBlogPostsResponse>> GetPosts()
        {
            var response = await _blogService.GetAllBlogPostsAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetBlogPostResponse>> GetPost(string id)
        {
            try
            {
                var post = await _blogService.GetBlogPostAsync(id);
                return Ok(post);
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
        [Authorize]
        public async Task<ActionResult<AddBlogPostResponse>> CreatePost([FromBody] AddBlogPostRequest request)
        {
            try
            {
                var userId = User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID claim is missing.");
                }
                var result = await _blogService.AddBlogPostAsync(request, userId);
                return Created("Blog", result);
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
        public async Task<ActionResult<UpdateBlogPostResponse>> UpdatePost(string id, [FromBody] UpdateBlogPostRequest request)
        {
            var userId = User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID claim is missing.");
            }
            try
            {
                var result = await _blogService.UpdateBlogPostAsync(request, userId);
                return Created("Blog", result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<DeleteBlogPostResponse>> DeletePost(string id)
        {
            var userId = User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID claim is missing.");
            }
            try
            {
                var result = await _blogService.DeleteBlogPostAsync(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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