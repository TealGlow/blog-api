using MongoDB.Bson;
using MongoDB.Driver;

public class CredentialsRepository : ICredentialsRepository
{
    private readonly IMongoCollection<Credentials> _collection;

    public CredentialsRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Credentials>("Credentials");
    }

    public async Task<bool> AddAsync(string hashedPassword, ObjectId userId)
    {
        var credentials = new Credentials
        {
            Id = userId,
            HashedPassword = hashedPassword
        };
        try
        {
            await _collection.InsertOneAsync(credentials);
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception (you can use a logging framework like Serilog or NLog)
            Console.WriteLine($"Error storing hashed password: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateAsync(string hashedPassword, ObjectId userId)
    {
        var filter = Builders<Credentials>.Filter.Eq(c => c.Id, userId);
        var update = Builders<Credentials>.Update.Set(c => c.HashedPassword, hashedPassword);
        try
        {
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error updating hashed password: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(ObjectId userId)
    {
        var filter = Builders<Credentials>.Filter.Eq(c => c.Id, userId);
        try
        {
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error deleting credentials: {ex.Message}");
            return false;
        }
    }
}
