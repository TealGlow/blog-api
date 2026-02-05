/// <summary>
/// Represents the request to add a new blog post. This class contains the necessary information required to create a new blog post, including the title, content, and the name of the author who created the post. Each property is initialized with an empty string to ensure that they are not null when the request is processed, which helps to prevent potential null reference exceptions during validation or when saving the new post to the database. This class serves as a data transfer object (DTO) that encapsulates the data needed for the AddBlogPost operation in a structured manner.
/// </summary>
public class AddBlogPostRequest
{
    /// <summary>
    /// The title of the blog post. This is a required field and cannot be empty. It represents the main heading or subject of the blog post and should provide a clear indication of the content that follows. The title is an important aspect of the blog post as it helps to attract readers and gives them an idea of what to expect from the post.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The main content of the blog post. This is a required field and cannot be empty. It represents the body of the blog post where the author can share their thoughts, ideas, information, or any other relevant content. The content should be well-structured and engaging to keep readers interested and encourage them to read through the entire post. It can include text, images, links, or any other media that enhances the overall message of the blog post.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// The username of the author who created the blog post. This is a required field and cannot be empty. It represents the identity of the person responsible for creating the blog post and can be used for attribution, accountability, and to foster a sense of community among readers. The CreatedBy property helps to establish a connection between the content and its creator, allowing readers to recognize and engage with the author of the blog post.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
}