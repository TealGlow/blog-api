/// <summary>
/// Represents the request to update an existing blog post. 
/// </summary>
public class UpdateBlogPostRequest
{
    /// <summary>
    /// The unique identifier for the blog post.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The title of the blog post. Nullable - only provided fields will be updated.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The main content of the blog post. Nullable - only provided fields will be updated.
    /// </summary>
    public string? Content { get; set; }
}