using MongoDB.Bson;

/// <summary>
/// The BlogService class serves as the business logic layer for managing blog posts in the application.
/// </summary>
public class BlogService : IBlogService
{
    private readonly IPostRepository _repo;
    private readonly IUserRepository _userRepo;

    public BlogService(IPostRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    /// <summary>
    /// Retrieves a single blog post by its unique identifier. 
    /// </summary>
    /// <param name="id">The unique identifier of the blog post to be retrieved. </param>
    /// <returns>A GetBlogPostResponse containing the details of the requested blog post.</returns>
    public async Task<GetBlogPostResponse> GetBlogPostAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            throw new ArgumentException("Invalid blog post id");

        var response = await _repo.GetByIdAsync(objectId);
        if (response == null) throw new Exception("Blog post now round.");

        return new GetBlogPostResponse
        {
            Id = response.Id.ToString(),
            Title = response.Title,
            Content = response.Content,
            CreatedBy = response.CreatedBy,
            CreatedAt = response.CreatedAt,
            UpdatedAt = response.UpdatedAt
        };
    }

    /// <summary>
    /// Retrieves all blog posts from the database. 
    /// </summary>
    /// <param name="request">The request object for retrieving all blog posts. </param
    /// <returns> A GetAllBlogPostsResponse containing a list of PostDto objects representing the blog posts retrieved from the database.</returns>
    public async Task<GetAllBlogPostsResponse> GetAllBlogPostsAsync()
    {
        var response = await _repo.GetAllAsync();
        if (response == null) throw new Exception("Blog posts not found.");

        return new GetAllBlogPostsResponse
        {
            BlogPosts = response.Select(post => new PostDto
            {
                Id = post.Id.ToString(),
                Title = post.Title,
                Content = post.Content,
                CreatedBy = post.CreatedBy,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt
            }).ToList()
        };
    }

    /// <summary>
    /// Adds a new blog post to the database. 
    /// </summary>
    /// <param name="request">
    /// The request object containing the necessary information to create a new blog post, including the title, content, and author of the post. The Title and Content fields are required and cannot be empty, while the CreatedBy field is optional and can be left empty if not provided. The method will validate the input data and throw an ArgumentException if any required fields are missing or invalid. Upon successful creation of the blog post, the method will return an AddBlogPostResponse containing the unique identifier of the newly created post.
    /// </param>
    /// <returns>
    /// An AddBlogPostResponse indicating the result of the add operation.
    /// </returns>
    public async Task<AddBlogPostResponse> CreateBlogPostAsync(AddBlogPostRequest request, string userId)
    {
        var user = await _userRepo.GetByIdAsync(new ObjectId(userId));
        if (user == null) throw new Exception("User not found.");

        var post = new Post
        {
            Title = request.Title,
            Content = request.Content,
            CreatedBy = user.UserName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AuthorId = user.Id.ToString()
        };

        var response = await _repo.CreateAsync(post);
        if (response == ObjectId.Empty)
            throw new Exception("Error adding post");

        return new AddBlogPostResponse
        {
            Id = response.ToString()
        };
    }

    /// <summary>
    /// Updates an existing blog post in the database. This method takes an UpdateBlogPostRequest
    /// </summary>
    /// <param name="request">The request object containing the necessary information to identify which blog post to update, 
    /// specifically the ID of the post, as well as the new values for the title, content, and author of the blog post. </param>
    /// <returns>An UpdateBlogPostResponse indicating the result of the update operation. </returns>
    public async Task<UpdateBlogPostResponse> UpdateBlogPostAsync(UpdateBlogPostRequest request, string userId)
    {
        if (!ObjectId.TryParse(request.Id, out var objectId))
        {
            throw new ArgumentException("Invalid Blog Post ID format.");
        }

        var existingPost = await _repo.GetByIdAsync(objectId);
        if (existingPost == null)
        {
            throw new KeyNotFoundException($"Blog post with ID {request.Id} not found.");
        }
        if (existingPost.AuthorId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to update this post.");
        }

        var response = await _repo.UpdateAsync(request, objectId);
        if (response == null)
            throw new Exception("Error updating post");

        if (response.MatchedCount == 0)
            throw new KeyNotFoundException("Blog post not found.");

        if (response.ModifiedCount == 0)
            throw new InvalidOperationException("No changes were made to the blog post.");

        return new UpdateBlogPostResponse
        {
            Id = request.Id
        };
    }

    /// <summary>
    /// Deletes an existing blog post from the database. This method takes a string ID as input
    /// and interacts with the IPostRepository to remove the corresponding blog post from the database. The method may include any necessary business logic or validations before performing the delete operation, such as checking if the blog post exists or if the user has the necessary permissions to delete the post. Upon successful completion, the method does not return any specific data, but it may throw an exception if the specified blog post is not found or if there is an error during the deletion process.
    /// </summary> 
    /// <param name="id">The unique identifier of the blog post to be deleted.</param>
    /// <returns>A DeleteBlogPostResponse indicating the result of the delete operation.</returns>
    public async Task<DeleteBlogPostResponse> DeleteBlogPostAsync(string id, string userId)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            throw new ArgumentException("Invalid blog post id");

        // find post to make sure it exists and to check if the user is authorized to delete it
        var post = await _repo.GetByIdAsync(objectId);

        if (post.AuthorId != userId)
            throw new UnauthorizedAccessException("You do not have permission to delete this post.");

        var response = await _repo.DeleteAsync(objectId);
        if (response.DeletedCount == 0)
            throw new KeyNotFoundException("Blog post not found.");

        return new DeleteBlogPostResponse();
    }
}