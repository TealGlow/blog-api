using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Token
{
    /// <summary>
    /// The unique identifier for the blog post.
    /// This is the primary key for the post in the database.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// The unique identifier of the author who created the blog post.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// The JWT token string that is issued to the user upon successful authentication. This token is
    /// </summary>
    public string JwtToken { get; set; } = string.Empty;

    /// <summary>
    /// Expiration date
    /// </summary>
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(60);
}
