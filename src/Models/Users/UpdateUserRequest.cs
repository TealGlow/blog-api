/// <summary>
/// Represents the request to update an existing user. 
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The username of the user. Nullable - only provided fields will be updated.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// The email of the user. Nullable - only provided fields will be updated.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Password of the user. Nullable - only provided fields will be updated.
    /// </summary>
    public string? Password { get; set; }
}