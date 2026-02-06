using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogManager _blogManager;

        public BlogController(BlogManager blogManager)
        {
            _blogManager = blogManager;
        }

        [HttpGet]
        public async Task<ActionResult<GetAllBlogPostsResponse>> GetPosts()
        {
            var response = await _blogManager.GetAllBlogPostsAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetBlogPostResponse>> GetPost(string id)
        {
            var post = await _blogManager.GetBlogPostAsync(id);
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<AddBlogPostResponse>> CreatePost([FromBody] AddBlogPostRequest request)
        {
            var result = await _blogManager.AddBlogPostAsync(request);
            return CreatedAtAction(nameof(GetPost), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateBlogPostResponse>> UpdatePost(string id, [FromBody] UpdateBlogPostRequest request)
        {
            request.Id = id;
            var result = await _blogManager.UpdateBlogPostAsync(request);
            return CreatedAtAction(nameof(GetPost), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteBlogPostResponse>> DeletePost(string id)
        {
            var result = await _blogManager.DeleteBlogPostAsync(id);
            return Ok(result);
        }
    }
}