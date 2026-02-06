using MongoDB.Bson;
using MongoDB.Driver;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserProfile> _collection;

    public UserRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<UserProfile>("Users");
    }

    public async Task<UserProfile?> GetByIdAsync(ObjectId id)
    {
        return await _collection
            .Find(u => u.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<UserProfile> AddAsync(UserProfile user)
    {
        await _collection.InsertOneAsync(user);
        return user;
    }
}
