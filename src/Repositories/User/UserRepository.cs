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

    public async Task<UserProfile> GetByUsernameOrEmail(UserProfile user)
    {
        var filter = Builders<UserProfile>.Filter.Or(
            Builders<UserProfile>.Filter.Eq(u => u.UserName, user.UserName),
            Builders<UserProfile>.Filter.Eq(u => u.Email, user.Email)
        );
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<ObjectId> AddAsync(UserProfile user)
    {
        await _collection.InsertOneAsync(user);
        return user.Id;
    }

    public async Task<UpdateResult> UpdateAsync(UpdateUserRequest request, ObjectId id)
    {
        var filter = Builders<UserProfile>.Filter.Eq(u => u.Id, id);
        var update = Builders<UserProfile>.Update.Set(u => u.UpdatedAt, DateTime.UtcNow);
        if (request.UserName != null)
        {
            update = update.Set(u => u.UserName, request.UserName);
        }
        if (request.Email != null)
        {
            update = update.Set(u => u.Email, request.Email);
        }
        if (request.Password != null)
        {
            update = update.Set(u => u.Password, request.Password);
        }

        try
        {
            return await _collection.UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating user: {ex.Message}");
        }
    }

    public async Task<UpdateResult> SoftDeleteAsync(ObjectId id)
    {
        var filter = Builders<UserProfile>.Filter.Eq(u => u.Id, id);
        //soft delete
        var update = Builders<UserProfile>.Update.Set(u => u.IsDeleted, true).Set(u => u.UpdatedAt, DateTime.UtcNow);
        try
        {
            return await _collection.UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting user: {ex.Message}");
        }
    }
}
