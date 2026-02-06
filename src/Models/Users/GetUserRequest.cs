public class GetUserRequest
{
    /// <summary>
    /// The unique identifier for the user. This is a required field and should correspond to an existing user in the database.
    /// </summary>
    public string Id { get; set; } = string.Empty;
}