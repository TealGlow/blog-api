using MongoDB.Bson;

public interface IUserRepository
{
    Task<UserProfile?> GetByIdAsync(ObjectId id);

    Task<UserProfile> AddAsync(UserProfile user);
}