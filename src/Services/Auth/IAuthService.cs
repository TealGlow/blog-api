using MongoDB.Bson;

public interface IAuthService
{
    Task<AuthLoginResponse> LoginAsync(AuthLoginRequest request);
}