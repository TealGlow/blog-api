/// <summary>
/// Represents the response returned when adding a new blog post. This class is currently empty because the AddBlogPost operation does not return any specific data upon successful completion. However, it serves as a placeholder for any future enhancements or additional information that may need to be included in the response, such as a success message, the ID of the newly created post, or any relevant metadata. By defining this response class, we maintain a consistent structure for our API responses and allow for easy expansion in the future without breaking existing functionality.
/// </summary>
public class AddBlogPostResponse
{
    /// <summary>
    /// The Id of the newly created post as a string.
    /// </summary>
    public string Id { get; set; } = string.Empty;
}