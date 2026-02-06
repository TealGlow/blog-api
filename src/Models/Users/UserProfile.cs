using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class UserProfile
{
    /// <summary>
    /// The unique identifier for the blog post.
    /// This is the primary key for the post in the database.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }


    /// <summary>
    /// The title of the blog post. This is a required field and cannot be empty.
    /// </summary>
    [Required]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email.
    /// </summary>
    [Required]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// The date and time when the blog post was created. This is automatically set to the current UTC time when a new post is created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The date and time when the blog post was last updated. This is automatically set to the current UTC time whenever the post is modified.
    /// It is nullable because a post may not have been updated after its initial creation.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// isDeleted is a boolean property that indicates whether the user profile has been marked as deleted.
    ///</summary>
    public bool IsDeleted { get; set; } = false;
}
