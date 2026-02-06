using MongoDB.Bson;

/// <summary>
/// The UserService class serves as the business logic layer for managing user profiles in the application. 
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves a user profile by its unique identifier. 
    /// </summary>
    /// <param name="id">The Id of the user profile to retrieve</param>
    /// <returns>GetUserResponse object.</returns>
    public async Task<GetUserResponse> GetUserProfileAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            throw new ArgumentException("Invalid blog post id");

        var user = await _repo.GetByIdAsync(objectId);
        if (user == null) throw new Exception("User not found");

        return new GetUserResponse
        {
            Id = user.Id.ToString(),
            UserName = user.UserName,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }

    /// <summary>
    /// Adds a new user profile to the database.
    /// </summary>
    /// <param name="request">User profile to add</param>
    /// <returns>Id of added user.</returns>
    public async Task<AddUserResponse> AddUserAsync(AddUserRequest request)
    {
        var userProfile = new UserProfile
        {
            UserName = request.UserName,
            Email = request.Email,
            Password = request.Password,
            CreatedAt = DateTime.UtcNow,
        };
        var response = await _repo.AddAsync(userProfile);
        if (response == ObjectId.Empty) throw new Exception("Failed to create user.");

        return new AddUserResponse
        {
            Id = response.ToString()
        };
    }
}