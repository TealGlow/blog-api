using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Credentials
{
    /// <summary>
    /// The unique identifier for the credentials.
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; }

    /// <summary>
    /// The user ID associated with these credentials. This is a reference to the UserProfile's Id.
    /// </summary>
    public ObjectId UserId { get; set; }

    /// <summary>
    /// The hashed password for the user. This should be securely hashed and salted.
    /// </summary>
    public string HashedPassword { get; set; } = string.Empty;
}
