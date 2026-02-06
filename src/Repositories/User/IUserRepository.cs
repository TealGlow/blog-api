using MongoDB.Bson;
using MongoDB.Driver;

public interface IUserRepository
{
    Task<UserProfile> GetByIdAsync(ObjectId id);

    Task<ObjectId> AddAsync(UserProfile user);

    Task<UpdateResult> UpdateAsync(UpdateUserRequest request, ObjectId id);

    Task<UserProfile> GetByUsernameOrEmail(UserProfile user);

    Task<UpdateResult> SoftDeleteAsync(ObjectId id);
}