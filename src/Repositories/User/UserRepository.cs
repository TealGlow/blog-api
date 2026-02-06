using MongoDB.Bson;
using MongoDB.Driver;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserProfile> _collection;

    public UserRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<UserProfile>("Users");
    }

    public async Task<UserProfile> GetByIdAsync(ObjectId id)
    {

        var result = await _collection
            .Find(u => u.Id == id)
            .FirstOrDefaultAsync();
        if (result == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }
        return new UserProfile
        {
            Id = result.Id,
            UserName = result.UserName,
            Email = result.Email,
            Password = result.Password,
            CreatedAt = result.CreatedAt
        };
    }

    public async Task<ObjectId> AddAsync(UserProfile user)
    {
        await _collection.InsertOneAsync(user);
        return user.Id;
    }
}
