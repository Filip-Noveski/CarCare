using CarCare.Processing.Abstract;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Models;

/// <summary>
/// A transferable User model.
/// </summary>
public class UserDto : Context
{
    /// <summary>
    /// The id of the user.
    /// </summary>
    public Guid Id
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The username of the user.
    /// </summary>
    public string Username
    {
        get => field;
        set
        {
            field = value; 
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The avatar image of the user.
    /// </summary>
    public BitmapImage Avatar
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Creates a new <see cref="UserDto"/> object.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="username">The username.</param>
    /// <param name="avatar">The avatar image.</param>
    public UserDto(Guid id, string username, BitmapImage avatar)
    {
        Id = id;
        Username = username;
        Avatar = avatar;
    }
}
