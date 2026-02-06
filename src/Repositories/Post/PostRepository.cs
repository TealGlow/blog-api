using MongoDB.Bson;
using MongoDB.Driver;

public class PostRepository : IPostRepository
{
    private readonly IMongoCollection<Post> _collection;

    public PostRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Post>("Posts");
    }

    public async Task<Post> GetByIdAsync(ObjectId id)
    {
        var result = await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (result == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }
        return new Post
        {
            Id = result.Id,
            Title = result.Title,
            Content = result.Content,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt,
            CreatedBy = result.CreatedBy
        };
    }

    public async Task<List<Post>> GetAllAsync()
    {
        var filter = Builders<Post>.Filter.Empty;
        var result = await _collection.Find(filter).ToListAsync();
        return result.Select(p => new Post
        {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            CreatedBy = p.CreatedBy
        }).ToList();
    }

    public async Task<ObjectId> AddAsync(Post post)
    {
        await _collection.InsertOneAsync(post);
        return post.Id;
    }

    public async Task<UpdateResult> UpdateAsync(UpdateBlogPostRequest updatePost, ObjectId id)
    {
        // check if there is actually an update to be made
        if (updatePost.Title == null && updatePost.Content == null)
        {
            throw new ArgumentException("At least one of Title or Content must be provided for update.");
        }
        // Find the existing post to ensure it exists before attempting an update
        var filter = Builders<Post>.Filter.Eq(u => u.Id, id);

        // Build the update definition based on which fields are provided in the request
        var update = Builders<Post>.Update.Set(u => u.UpdatedAt, DateTime.UtcNow);
        if (updatePost.Title != null)
        {
            update = update.Set(p => p.Title, updatePost.Title);
        }
        if (updatePost.Content != null)
        {
            update = update.Set(p => p.Content, updatePost.Content);
        }

        try
        {
            return await _collection.UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating post: {ex.Message}");
            throw;
        }
    }

    public async Task<DeleteResult> DeleteAsync(ObjectId id)
    {
        return await _collection.DeleteOneAsync(p => p.Id == id);
    }
}
