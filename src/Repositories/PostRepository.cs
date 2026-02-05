using MongoDB.Bson;
using MongoDB.Driver;

public class PostRepository : IPostRepository<PostDto>
{
    private readonly IMongoCollection<Post> _collection;

    public PostRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Post>("Posts");
    }

    public async Task<GetBlogPostResponse> GetByIdAsync(string id)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new Exception("Invalid post ID format");
            }

            var post = await _collection.Find(p => p.Id == objectId).FirstOrDefaultAsync();

            if (post == null)
            {
                throw new Exception("Post not found");
            }

            return new GetBlogPostResponse
            {
                Id = post.Id.ToString(),
                Title = post.Title,
                Content = post.Content,
                CreatedBy = post.CreatedBy,
                CreatedAt = post.CreatedAt
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the blog post: {ex.Message}", ex);
        }
    }

    public async Task<GetAllBlogPostsResponse> GetAllAsync()
    {
        try
        {
            var posts = await _collection.Find(_ => true).ToListAsync();

            var postDtos = posts.Select(p => new PostDto
            {
                Id = p.Id.ToString(),
                Title = p.Title,
                Content = p.Content,
                CreatedBy = p.CreatedBy,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();

            return new GetAllBlogPostsResponse
            {
                BlogPosts = postDtos
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving blog posts: {ex.Message}", ex);
        }
    }

    public async Task<AddBlogPostResponse> AddAsync(AddBlogPostRequest request)
    {
        try
        {
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            await _collection.InsertOneAsync(post);

            return new AddBlogPostResponse
            {
                Id = post.Id.ToString()
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while adding the blog post: {ex.Message}", ex);
        }
    }

    public async Task<UpdateBlogPostResponse> UpdateAsync(UpdateBlogPostRequest request)
    {
        try
        {
            if (!ObjectId.TryParse(request.Id, out var objectId))
            {
                throw new Exception("Invalid post ID format");
            }

            var updateBuilder = Builders<Post>.Update;
            var updates = new List<UpdateDefinition<Post>>
            {
                updateBuilder.Set(p => p.UpdatedAt, DateTime.UtcNow)
            };

            // Only add updates for fields that were provided
            if (!string.IsNullOrEmpty(request.Title))
            {
                updates.Add(updateBuilder.Set(p => p.Title, request.Title));
            }

            if (!string.IsNullOrEmpty(request.Content))
            {
                updates.Add(updateBuilder.Set(p => p.Content, request.Content));
            }

            var combinedUpdate = updateBuilder.Combine(updates);
            var result = await _collection.UpdateOneAsync(p => p.Id == objectId, combinedUpdate);

            if (result.MatchedCount == 0)
            {
                throw new Exception("Post not found");
            }

            return new UpdateBlogPostResponse
            {
                Id = request.Id,
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while updating the blog post: {ex.Message}", ex);
        }
    }

    public async Task<DeleteBlogPostResponse> DeleteAsync(string id)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new Exception("Invalid post ID format");
            }

            var result = await _collection.DeleteOneAsync(p => p.Id == objectId);

            if (result.DeletedCount == 0)
            {
                throw new Exception("Post not found");
            }

            return new DeleteBlogPostResponse
            {
                Id = id
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the blog post: {ex.Message}", ex);
        }
    }
}
