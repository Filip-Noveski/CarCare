using CarCare.Processing.Models.Communication;
using CarCare.Processing.Models.Core;
using OneOf;

namespace CarCare.Processing.Interfaces.Service;

internal interface IUserService
{
    Task RegisterAsync(User user);

    Task DeleteAsync(Guid id);

    Task DeleteAsync(string username);

    Task UpdateAsync(User user);

    Task<OneOf<User, LoginError>> LoginAsync(string username, string password);

    Task<User> GetUserAsync(Guid id);

    Task<User> GetUserAsync(string username);

    Task<IEnumerable<User>> GetUsersAsync();
}
