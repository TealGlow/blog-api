using System.IdentityModel.Tokens.Jwt;
using MongoDB.Bson;

public interface ITokenRepository
{
    Task<bool> AddAsync(JwtSecurityToken token, ObjectId userId);

    Task<bool> DeleteAsync(ObjectId userId);
}