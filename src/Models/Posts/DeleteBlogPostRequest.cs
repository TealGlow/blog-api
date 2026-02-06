/// <summary>
/// Represents the request to delete an existing blog post. 
/// </summary>
public class DeletelogPostRequest
{
    /// <summary>
    /// The unique identifier for the blog post.
    /// </summary>
    public string Id { get; set; } = string.Empty;
}