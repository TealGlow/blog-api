public class AuthLoginResponse
{
    /// <summary>
    /// Indicates whether the login attempt was successful.
    /// </summary>

    public bool Success { get; set; } = true;

    /// <summary>
    /// Bearer JWT
    /// </summary>
    public string Token { get; set; } = string.Empty;
}