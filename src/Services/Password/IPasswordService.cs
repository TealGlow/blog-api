
using MongoDB.Bson;

public interface IPasswordService
{
    string HashPassword(string password);

    bool VerifyPassword(string enteredPassword, string storedHash);

    Task<string> GetHashedPasswordAsync(ObjectId userId);

    Task<bool> StoreHashedPasswordAsync(string hashedPassword, ObjectId userId);
}