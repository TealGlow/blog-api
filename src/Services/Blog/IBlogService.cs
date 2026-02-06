public interface IBlogService
{
    Task<GetBlogPostResponse> GetBlogPostAsync(string id);
    
    Task<GetAllBlogPostsResponse> GetAllBlogPostsAsync();

    Task<AddBlogPostResponse> AddBlogPostAsync(AddBlogPostRequest request);

    Task<UpdateBlogPostResponse> UpdateBlogPostAsync(UpdateBlogPostRequest request);

    Task<DeleteBlogPostResponse> DeleteBlogPostAsync(string id);
}