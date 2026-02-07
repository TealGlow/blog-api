using MongoDB.Bson;

public interface ICredentialsRepository
{
    Task<bool> AddAsync(string hashedPassword, ObjectId userId);

    Task<bool> UpdateAsync(string hashedPassword, ObjectId userId);

    Task<string> GetHashedPasswordAsync(ObjectId userId);

    Task<bool> DeleteAsync(ObjectId userId);
}