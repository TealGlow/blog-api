using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;

/// <summary>
/// The UserService class serves as the business logic layer for managing user profiles in the application. 
/// </summary>
public class AuthService : IAuthService
{
    private readonly IPasswordService _passwordService;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthService(IPasswordService passwordService, IUserService userService, IConfiguration configuration)
    {
        _passwordService = passwordService;
        _userService = userService;
        _configuration = configuration;
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

        var token = GenerateJwtToken(request.UserName);
        return new AuthLoginResponse
        {
            Success = true,
            Token = token
        };
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // Add custom claims like roles here
            new Claim(ClaimTypes.Role, "User")
        };

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? "default_really_long_secret_key_that_should_be_changed"));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(30); // Set token expiration time

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}