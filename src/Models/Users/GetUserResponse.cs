/// <summary>
/// Represents the request to get a user profile by id.
/// </summary>
public class GetUserResponse
{

    /// <summary>
    /// id.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// User name
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// date user created
    /// </summary>
    public DateTime CreatedAt { get; set; }
}