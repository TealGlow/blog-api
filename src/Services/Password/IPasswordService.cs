
using MongoDB.Bson;

public interface IPasswordService
{
    string HashPassword(string password);

    bool VerifyPassword(string enteredPassword, string storedHash);

    Task<bool> StoreHashedPasswordAsync(string password, ObjectId userId);
}