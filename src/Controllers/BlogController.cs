using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
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
            var post = await _blogService.GetBlogPostAsync(id);
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<AddBlogPostResponse>> CreatePost([FromBody] AddBlogPostRequest request)
        {
            var result = await _blogService.AddBlogPostAsync(request);
            return CreatedAtAction(nameof(GetPost), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateBlogPostResponse>> UpdatePost(string id, [FromBody] UpdateBlogPostRequest request)
        {
            request.Id = id;
            var result = await _blogService.UpdateBlogPostAsync(request);
            return CreatedAtAction(nameof(GetPost), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteBlogPostResponse>> DeletePost(string id)
        {
            var result = await _blogService.DeleteBlogPostAsync(id);
            return Ok(result);
        }
    }
}