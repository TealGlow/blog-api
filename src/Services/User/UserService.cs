using MongoDB.Bson;
using MongoDB.Driver;

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
            UpdatedAt = DateTime.UtcNow
        };

        //check if username or email already exists
        var existingUser = await _repo.GetByUsernameOrEmail(userProfile);
        if (existingUser != null) throw new ArgumentException("A user with the same username or email already exists.");

        var response = await _repo.AddAsync(userProfile);
        if (response == ObjectId.Empty) throw new Exception("Failed to create user.");

        return new AddUserResponse
        {
            UserName = request.UserName,
            Id = response.ToString()
        };
    }

    /// <summary>
    /// Updates an existing user profile in the database.
    /// </summary>
    /// <param name="id">The Id of the user profile to update</param>
    /// <param name="request">The updated user profile data</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
    {
        if (!ObjectId.TryParse(request.Id, out var objectId))
            throw new ArgumentException("Invalid user id");

        var existingUser = await _repo.GetByIdAsync(objectId);
        if (existingUser == null) throw new Exception("User not found");

        // TODO: Validate user is authorized to update this profile, Only a user can update their own profile, or an admin can update any profile.
        // This will be after we implement authentication and authorization, but we can add a placeholder check here for now.

        // Update only provided fields
        var response = await _repo.UpdateAsync(request, objectId);
        if (response == null) throw new Exception("User not found");
        if (response.ModifiedCount == 0) throw new Exception("No changes were made to the user profile.");

        return new UpdateUserResponse
        {
            Id = request.Id
        };
    }

    /// <summary>
    /// Deletes an existing user profile from the database.
    /// </summary>
    /// <param name="id">The Id of the user profile to delete</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    public async Task<DeleteUserResponse> DeleteUserAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            throw new ArgumentException("Invalid user id");

        var existingUser = await _repo.GetByIdAsync(objectId);
        if (existingUser == null) throw new Exception("User not found");

        // TODO: Validate user is authorized to delete this profile, Only a user can delete their own profile, or an admin can delete any profile.
        // We want only admins to be able to delete user profiles, so we can add a placeholder check here for now, and implement proper authorization after we have authentication in place.
        var result = await _repo.SoftDeleteAsync(objectId);
        if (result == null) throw new Exception("User not found");

        return new DeleteUserResponse
        {
            Id = id
        };
    }
}