using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;
using MongoDB.Driver;

public interface ITokenRepository
{
    Task<bool> AddAsync(JwtSecurityToken token, ObjectId userId);

    Task<bool> DeleteAsync(ObjectId userId);
}