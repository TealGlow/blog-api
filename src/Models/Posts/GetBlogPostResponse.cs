using MongoDB.Bson.Serialization.Attributes;

/// <summary>
/// Represents the response returned when retrieving a single blog post by its unique identifier.
/// This class contains all the relevant information about the blog post, including its ID, title,
/// </summary>
public class GetBlogPostResponse
{
    // <summary>
    /// The unique identifier for the blog post.
    /// </summary>
    [BsonId]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The title of the blog post.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The main content of the blog post. This is a required field and cannot be empty.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// The username of the author who created the blog post.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// The date and time when the blog post was created. This is automatically set to the current UTC time when a new post is created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date and time when the blog post was last updated. This is automatically set to the current UTC time whenever the post is modified.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}