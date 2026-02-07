using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Post
{
    /// <summary>
    /// The unique identifier for the blog post.
    /// This is the primary key for the post in the database.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// The username of the author who created the blog post.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier of the author who created the blog post.
    /// </summary>
    public string AuthorId { get; set; } = string.Empty;

    /// <summary>
    /// The title of the blog post. This is a required field and cannot be empty.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The main content of the blog post. This is a required field and cannot be empty.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// The date and time when the blog post was created. This is automatically set to the current UTC time when a new post is created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The date and time when the blog post was last updated. This is automatically set to the current UTC time whenever the post is modified.
    /// It is nullable because a post may not have been updated after its initial creation.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
