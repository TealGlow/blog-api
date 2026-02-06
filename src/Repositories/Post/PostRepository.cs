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
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Post>> GetAllAsync()
    {
            return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<ObjectId> AddAsync(Post request)
    {
        await _collection.InsertOneAsync(request);
        return request.Id;
    }

    public async Task<UpdateResult> UpdateAsync(UpdateBlogPostRequest request, ObjectId id)
    {
        
        var filter = Builders<Post>.Filter.Eq(u => u.Id, id);
        

        var update = Builders<Post>.Update
            .Set(u => u.CreatedAt, DateTime.UtcNow);
        if(request.Title != null)
        {
            update.Set(p=>p.Title, request.Title);
        }
        if(request.Content != null)
        {
            update.Set(p=>p.Content, request.Content);
        }


        return await _collection.UpdateOneAsync(filter, update);     
    }

    public async Task<DeleteResult> DeleteAsync(ObjectId id)
    {
        return await _collection.DeleteOneAsync(p => p.Id == id);
    }
}
