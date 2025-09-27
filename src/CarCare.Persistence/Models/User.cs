namespace CarCare.Persistence.Models;

/// <summary>
/// A database user model.
/// </summary>
public class User
{
    /// <summary>
    /// The id of the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The username of the user.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The hashed password of the user.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// An optional avatar image.
    /// </summary>
    public byte[]? Avatar { get; set; }

    /// <summary>
    /// Creates a new instance of the <see cref="User"/> class.
    /// </summary>
    public User()
    {
        Id = Guid.Empty;
        Username = string.Empty;
        Password = string.Empty;
        Avatar = null;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="username">The username.</param>
    /// <param name="password">The hashed password.</param>
    public User(Guid id, string username, string password)
    {
        Id = id;
        Username = username;
        Password = password;
        Avatar = null;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="username">The username.</param>
    /// <param name="password">The hashed password.</param>
    /// <param name="avatar">The bytes of the avatar image.</param>
    public User(Guid id, string username, string password, byte[]? avatar) : this(id, username, password)
    {
        Avatar = avatar;
    }
}
