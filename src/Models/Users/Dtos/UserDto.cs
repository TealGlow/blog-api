using System.ComponentModel.DataAnnotations;

public class UserDto
{
    /// <summary>
    /// User id
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// User name
    /// </summary>
    [Required]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// User email
    /// </summary>
    [Required]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User password
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// date user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}