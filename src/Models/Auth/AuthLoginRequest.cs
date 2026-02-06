public class AuthLoginRequest
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The username of the user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The email of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// request password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}