
public interface IUserProfileService
{
    Task<GetUserResponse> GetUserProfileAsync(string id);
    Task<UserProfile> GetUserProfileByUsernameOrEmailAsync(string usernameOrEmail);
    Task<AddUserResponse> AddUserAsync(AddUserRequest request);
    Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);
    Task<DeleteUserResponse> DeleteUserAsync(string id);
}