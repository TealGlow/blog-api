
public interface IBlogService
{
    Task<GetBlogPostResponse> GetBlogPostAsync(string id);

    Task<GetAllBlogPostsResponse> GetAllBlogPostsAsync();

    Task<AddBlogPostResponse> AddBlogPostAsync(AddBlogPostRequest request, string userId);

    Task<UpdateBlogPostResponse> UpdateBlogPostAsync(UpdateBlogPostRequest request, string userId);

    Task<DeleteBlogPostResponse> DeleteBlogPostAsync(string id, string userId);
}