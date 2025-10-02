using CarCare.Processing.Models;

namespace CarCare.Processing.Interfaces.Session;

internal interface IUserSession
{
    UserDto? User { get; }

    void LoginUser(UserDto user);

    void LogoutUser();

    bool IsAuthenticated(User user);

    bool IsAuthenticated(string username);

    bool IsAuthenticated(Guid id);
}
