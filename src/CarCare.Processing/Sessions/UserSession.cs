using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models;

namespace CarCare.Processing.Sessions;

internal class UserSession : IUserSession
{
    public UserDto? User { get; private set; }

    public UserSession()
    {
        User = null;
    }

    public void LoginUser(UserDto user)
    {
        if (User is not null)
        {
            return;
        }

        User = user;
    }

    public void LogoutUser()
    {
        User = null;
    }

    public bool IsAuthenticated(Guid id)
    {
        if (User is null)
        {
            return false;
        }

        return User.Id == id;
    }

    public bool IsAuthenticated(string username)
    {
        if (User is null)
        {
            return false;
        }

        return User.Username == username;
    }

    public bool IsAuthenticated(User user)
    {
        if (User is null)
        {
            return false;
        }

        return User.Id == user.Id;
    }
}
