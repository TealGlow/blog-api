
/// <summary>
/// Represents the response returned when retrieving all blog posts. This class contains a list of PostDto objects, which represent the individual blog posts retrieved from the database. Each PostDto includes details such as the post's ID, title, content, author, and timestamps for creation and updates. The BlogPosts property is initialized as an empty list to ensure that it is always ready to hold the retrieved blog posts, even if no posts are found in the database.
/// </summary>
public class GetAllBlogPostsResponse
{
    /// <summary>
    /// A list of PostDto objects representing the blog posts retrieved from the database. Each PostDto contains details about an individual blog post, including its ID, title, content, author, and timestamps for creation and updates. This property is initialized as an empty list to ensure that it can safely hold the retrieved blog posts without encountering null reference issues, even if no posts are found in the database.
    /// </summary>
    public List<Post> BlogPosts { get; set; } = new List<Post>();
}