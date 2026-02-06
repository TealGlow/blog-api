/// <summary>
/// Represents the request to delete an existing user. 
/// </summary>
public class DeleteUserPostRequest
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public string Id { get; set; } = string.Empty;
}