using MongoDB.Bson;

public interface IUserService
{
    Task<GetUserResponse> GetUserProfileAsync(string id);
    Task<AddUserResponse> AddUserAsync(AddUserRequest request);
    Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);

    Task<DeleteUserResponse> DeleteUserAsync(string id);

    Task<UserLoginResponse> LoginAsync(UserLoginRequest request);
}