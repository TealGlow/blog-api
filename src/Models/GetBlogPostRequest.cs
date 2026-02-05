using MongoDB.Bson.Serialization.Attributes;

/// <summary>
/// Represents the request to retrieve a single blog post by its unique identifier. This class contains the necessary information to identify which blog post to retrieve, specifically the ID of the post. The ID is expected to be an integer that corresponds to the unique identifier of the blog post in the database.
/// </summary>
public class GetBlogPostRequest
{
    /// <summary>
    /// The unique identifier for the blog post to be retrieved. This is a required field and must be provided when making the request. The value should correspond to the ID of an existing blog post in the database.
    /// </summary>
    [BsonId]
    public int Id { get; set; }
}