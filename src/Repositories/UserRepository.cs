using MongoDB.Bson;
using MongoDB.Driver;

public class UserRepository : IUserRepository<UserDto>
{
    private readonly IMongoCollection<UserProfile> _collection;

    public UserRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<UserProfile>("Users");
    }

    public async Task<GetUserResponse> GetByIdAsync(string id)
    {
        try
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new Exception("Invalid user ID format");
            }

            var user = await _collection.Find(p => p.Id == objectId).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return new GetUserResponse
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the user: {ex.Message}", ex);
        }
    }

    public async Task<AddUserResponse> AddAsync(AddUserRequest request)
    {
        try
        {
            var user = new UserProfile
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password
            };

            await _collection.InsertOneAsync(user);

            return new AddUserResponse
            {
                Id = user.Id.ToString()
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while adding the user: {ex.Message}", ex);
        }
    }
}
