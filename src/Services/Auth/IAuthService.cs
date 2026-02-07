
public interface IAuthService
{
    Task<AuthLoginResponse> LoginAsync(AuthLoginRequest request);
    Task<bool> LogoutAsync(AuthLogoutRequest request);
}