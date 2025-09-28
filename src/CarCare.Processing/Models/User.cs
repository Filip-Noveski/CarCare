using CarCare.Persistence.Models;
using Microsoft.AspNetCore.Identity;

namespace CarCare.Processing.Models;

/// <summary>
/// A constrained user model.
/// </summary>
public class User
{
    /// <summary>
    /// The id of the <see cref="User"/>.
    /// </summary>
    public Guid Id { get; internal set; }

    /// <summary>
    /// The username of the <see cref="User"/>.
    /// </summary>
    public string Username { get; internal set; }

    /// <summary>
    /// The hashed password of the <see cref="User"/>.
    /// </summary>
    public string Password { get; internal set; }

    /// <summary>
    /// The avatar image of the <see cref="User"/>.
    /// </summary>
    public byte[]? Avatar { get; internal set; }

    internal User(Guid id, string username, string passwordHash)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("The id cannot be empty", nameof(id));
        }
        Id = id;

        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("The username cannot be null or empty", nameof(username));
        }
        Username = username;

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("The password cannot be null or empty", nameof(passwordHash));
        }
        Password = passwordHash;

        Avatar = null;
    }

    internal User(Guid id, string username, string passwordHash, byte[] avatar) 
        : this(id, username, passwordHash)
    {
        Avatar = avatar;
    }

    internal User(UserDao dao)
    {
        Id = dao.Id;
        Username = dao.Username;
        Password = dao.Password;
        Avatar = dao.Avatar;
    }

    internal UserDao ToDao()
    {
        return new(Id, Username, Password, Avatar);
    }

    /// <summary>
    /// Creates a new <see cref="User"/> object.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <param name="username">The username of the user.</param>
    /// <param name="plainPassword">The plaintext password of the user.</param>
    /// <param name="hasher">A password hashing service.</param>
    public static User Create(Guid id, string username, string plainPassword, IPasswordHasher<User> hasher)
    {
        string hashedPassword = hasher.HashPassword(null!, plainPassword);
        return new(id, username, hashedPassword);
    }

    /// <summary>
    /// Creates a new <see cref="User"/> object.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <param name="username">The username of the user.</param>
    /// <param name="plainPassword">The plaintext password of the user.</param>
    /// <param name="avatar">The bytes of the avatar image.</param>
    /// <param name="hasher">A password hashing service.</param>
    public static User Create(
        Guid id, string username, string plainPassword, byte[] avatar, IPasswordHasher<User> hasher)
    {
        string hashedPassword = hasher.HashPassword(null!, plainPassword);
        return new(id, username, hashedPassword, avatar);
    }
}
