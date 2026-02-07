using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;
using MongoDB.Driver;

public class TokenRepository : ITokenRepository
{
    private readonly IMongoCollection<Token> _collection;

    public TokenRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Token>("Tokens");
    }

    public async Task<bool> AddAsync(JwtSecurityToken token, ObjectId userId)
    {
        var tokenDocument = new Token
        {
            UserId = userId.ToString(),
            JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = token.ValidTo
        };
        try
        {
            await _collection.InsertOneAsync(tokenDocument);
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception (you can use a logging framework like Serilog or NLog)
            Console.WriteLine($"Error storing token: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(ObjectId userId)
    {
        var filter = Builders<Token>.Filter.Eq(t => t.UserId, userId.ToString());
        try
        {
            var result = await _collection.DeleteManyAsync(filter);
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error deleting token: {ex.Message}");
            return false;
        }
    }
}
