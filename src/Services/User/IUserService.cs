using MongoDB.Bson;

public interface IUserService
{
    Task<GetUserResponse> GetUserProfileAsync(string id);
    Task<AddUserResponse> AddUserAsync(AddUserRequest request);
}