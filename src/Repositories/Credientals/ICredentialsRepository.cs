using MongoDB.Bson;
using MongoDB.Driver;

public interface ICredentialsRepository
{
    Task<bool> AddAsync(string hashedPassword, ObjectId userId);

    Task<bool> UpdateAsync(string hashedPassword, ObjectId userId);

    Task<bool> DeleteAsync(ObjectId userId);
}