using MongoDB.Bson;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

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

        return new AddUserResponse
        {
            Id = response.Id.ToString()
        };
    }
}