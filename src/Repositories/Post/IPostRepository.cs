using MongoDB.Bson;
using MongoDB.Driver;

public interface IPostRepository
{
    Task<Post> GetByIdAsync(ObjectId id);

    Task<List<Post>> GetAllAsync();

    Task<ObjectId> CreateAsync(Post request);

    Task<UpdateResult> UpdateAsync(UpdateBlogPostRequest request, ObjectId id);

    Task<DeleteResult> DeleteAsync(ObjectId id);
}

