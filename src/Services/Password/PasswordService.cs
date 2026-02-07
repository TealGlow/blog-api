using MongoDB.Bson;

/// <summary>
/// The PasswordService class is responsible for handling password-related operations, such as hashing and verifying passwords.
/// </summary>
public class PasswordService : IPasswordService
{

    private readonly ICredentialsRepository _repo;

    public PasswordService(ICredentialsRepository repo)
    {
        _repo = repo;
    }
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    public bool VerifyPassword(string enteredPassword, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
    }

    public async Task<bool> StoreHashedPasswordAsync(string hashedPassword, ObjectId userId)
    {
        // store hashed password and user id in credentials collection
        return await _repo.AddAsync(hashedPassword, userId);
    }

    public async Task<string> GetHashedPasswordAsync(ObjectId userId)
    {
        // get hashed password from credentials collection based on username or email
        return await _repo.GetHashedPasswordAsync(userId);
    }
}