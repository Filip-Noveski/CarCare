using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models;
using Microsoft.AspNetCore.Identity;
using OneOf;
using System.Diagnostics;

namespace CarCare.Processing.Services;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _hasher;

    public UserService(IUserRepository userRepository, IPasswordHasher<User> hasher)
    {
        _userRepository = userRepository;
        _hasher = hasher;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }

    public async Task DeleteAsync(string username)
    {
        await _userRepository.DeleteAsync(username);
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        UserDao user = await _userRepository.GetUserAsync(id);
        return new(user);
    }

    public async Task<User> GetUserAsync(string username)
    {
        UserDao user = await _userRepository.GetUserAsync(username);
        return new(user);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        IEnumerable<UserDao> users = await _userRepository.GetUsersAsync();
        return users.Select(x => new User(x));
    }

    public async Task<OneOf<User, LoginError>> LoginAsync(string username, string password)
    {
        UserDao user = await _userRepository.GetUserAsync(username);
        PasswordVerificationResult result = _hasher.VerifyHashedPassword(null!, user.Password, password);
        return result switch
        {
            PasswordVerificationResult.Failed => new LoginError("Incorrect password"),
            PasswordVerificationResult.Success => new User(user),
            PasswordVerificationResult.SuccessRehashNeeded => throw new Exception("Needs handling"),
            _ => throw new UnreachableException()
        };
    }

    public async Task RegisterAsync(User user)
    {
        UserDao dao = user.ToDao();
        await _userRepository.AddAsync(dao);
    }

    public async Task UpdateAsync(User user)
    {
        UserDao dao = user.ToDao();
        await _userRepository.UpdateAsync(dao);
    }
}
