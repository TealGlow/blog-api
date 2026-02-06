using MongoDB.Bson;

public interface IUserRepository
{
    Task<UserProfile> GetByIdAsync(ObjectId id);

    Task<ObjectId> AddAsync(UserProfile user);
}