using CarCare.Persistence.Models;
using CarCare.Processing.Interfaces.Service;
using Microsoft.AspNetCore.Identity;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Models;

internal class User
{
    public Guid Id { get; internal set; }

    public string Username { get; internal set; }

    public string Password { get; internal set; }

    public BitmapImage Avatar { get; internal set; }

    public bool HasCustomAvatar { get; internal set; }

    internal User(Guid id, string username, string passwordHash, BitmapImage avatar, bool hasCustomAvatar)
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

        Avatar = avatar;
        HasCustomAvatar = hasCustomAvatar;
    }

    internal User(UserDao dao, IBitmapCreatorService bitmapCreator)
    {
        Id = dao.Id;
        Username = dao.Username;
        Password = dao.Password;
        Avatar = dao.Avatar switch
        {
            null => bitmapCreator.GetGenericAvatar(dao.Username),
            _ => bitmapCreator.ConvertToBitmap(dao.Avatar)
        };
        HasCustomAvatar = dao.Avatar is not null;
    }

    public static User Create(string username, string plainPassword, IPasswordHasher<User> hasher, IBitmapCreatorService bitmapCreator)
    {
        string hashedPassword = hasher.HashPassword(null!, plainPassword);
        return new(Guid.NewGuid(), username, hashedPassword, bitmapCreator.GetGenericAvatar(username), false);
    }

    public static User Create(string username, string plainPassword, byte[] avatar, IPasswordHasher<User> hasher, IBitmapCreatorService bitmapCreator)
    {
        string hashedPassword = hasher.HashPassword(null!, plainPassword);
        return new(Guid.NewGuid(), username, hashedPassword, bitmapCreator.ConvertToBitmap(avatar), true);
    }

    public static User Create(string username, string plainPassword, BitmapImage avatar, IPasswordHasher<User> hasher)
    {
        string hashedPassword = hasher.HashPassword(null!, plainPassword);
        return new(Guid.NewGuid(), username, hashedPassword, avatar, true);
    }

    public UserDto ToDto()
    {
        return new(Id, Username, Avatar);
    }

    internal UserDao ToDao(IBitmapCreatorService bitmapCreator)
    {
        return new(Id, Username, Password, HasCustomAvatar ? bitmapCreator.ConvertToBinary(Avatar) : null);
    }
}
