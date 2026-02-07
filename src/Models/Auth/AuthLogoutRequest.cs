public class AuthLogoutRequest
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The username of the user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;
}