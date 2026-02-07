using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

/// <summary>
/// The UserService class serves as the business logic layer for managing user profiles in the application. 
/// </summary>
public class AuthService : IAuthService
{
    private readonly IPasswordService _passwordService;
    private readonly IUserService _userService;

    private readonly ITokenRepository _tokenRepo;

    public AuthService(IPasswordService passwordService, IUserService userService, ITokenRepository tokenRepo)
    {
        _passwordService = passwordService;
        _userService = userService;
        _tokenRepo = tokenRepo;
    }

    /// <summary>
    /// Authenticates a user and returns a response indicating success or failure of the login attempt.
    /// </summary>
    /// <param name="request">The login request containing the username/email and password.</param>
    /// <returns>UserLoginResponse indicating the result of the login attempt.</returns>
    public async Task<AuthLoginResponse> LoginAsync(AuthLoginRequest request)
    {
        // get hashed password from credentials collection based on username or email
        // find user based on username or email to get user id, then use user id to get hashed password from credentials collection
        var user = await _userService.GetUserProfileByUsernameOrEmailAsync(request.UserName);
        var hashedPassword = await _passwordService.GetHashedPasswordAsync(user.Id);

        // validate password
        var passwordValid = _passwordService.VerifyPassword(request.Password, hashedPassword);
        if (!passwordValid) throw new ArgumentException("Invalid password.");

        var token = GenerateJwtToken(user);
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Failed to generate JWT token.");
        }

        // store the token in the database or cache if needed for future validation (optional, depending on your token management strategy)
        var tokenStored = await _tokenRepo.AddAsync(new JwtSecurityToken(token), user.Id);
        if (!tokenStored)
        {
            throw new Exception("Failed to store JWT token.");
        }

        return new AuthLoginResponse
        {
            Success = true,
            Token = token
        };
    }

    private string GenerateJwtToken(UserProfile user)
    {
        var claims = new[]
        {
            // Use the full namespace to avoid the BinaryReader conflict
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "User")
        };

        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                      ?? "a_very_long_fallback_secret_32_chars";
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                        ?? "YourApiIssuer";
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                        ?? "YourApiClient";

        var keyString = jwtSecret ?? "default_secret_key";
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience, // Match "JwtSettings"
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}