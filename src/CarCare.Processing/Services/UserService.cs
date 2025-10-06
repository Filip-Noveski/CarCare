using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Communication;
using CarCare.Processing.Models.Core;
using Microsoft.AspNetCore.Identity;
using OneOf;
using System.Diagnostics;

namespace CarCare.Processing.Services;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IBitmapCreatorService _bitmapService;
    private readonly IUserSession _userSession;
    private readonly IUserSettingsService _settingsService;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher<User> hasher,
        IBitmapCreatorService bitmapService,
        IUserSession userSession,
        IUserSettingsService settingsService)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _bitmapService = bitmapService;
        _userSession = userSession;
        _settingsService = settingsService;
    }

    public async Task DeleteAsync(Guid id)
    {
        if (!_userSession.IsAuthenticated(id))
        {
            return;
        }

        await _settingsService.DeleteAsync(id);
        await _userRepository.DeleteAsync(id);
    }

    public async Task DeleteAsync(string username)
    {
        if (!_userSession.IsAuthenticated(username))
        {
            return;
        }

        UserDao user = await _userRepository.GetUserAsync(username);
        await _settingsService.DeleteAsync(user.Id);
        await _userRepository.DeleteAsync(username);
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        UserDao user = await _userRepository.GetUserAsync(id);
        return new(user, _bitmapService);
    }

    public async Task<User> GetUserAsync(string username)
    {
        UserDao user = await _userRepository.GetUserAsync(username);
        return new(user, _bitmapService);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        IEnumerable<UserDao> users = await _userRepository.GetUsersAsync();
        return users.Select(x => new User(x, _bitmapService));
    }

    public async Task<OneOf<User, LoginError>> LoginAsync(string username, string password)
    {
        UserDao user = await _userRepository.GetUserAsync(username);
        PasswordVerificationResult result = _hasher.VerifyHashedPassword(null!, user.Password, password);

        if (result is PasswordVerificationResult.Failed)
        {
            return new LoginError("Incorrect password");
        }

        if (result is PasswordVerificationResult.Success)
        {
            User loggedIn = new(user, _bitmapService);
            _userSession.LoginUser(loggedIn.ToDto());
            return loggedIn;
        }

        throw new UnreachableException();
    }

    public async Task RegisterAsync(User user)
    {
        UserDao dao = user.ToDao(_bitmapService);
        UserSettings settings = UserSettings.CreateDefault(user.Id);
        await _userRepository.AddAsync(dao);
        await _settingsService.AddAsync(settings);
        _userSession.LoginUser(user.ToDto());
    }

    public async Task UpdateAsync(User user)
    {
        if (!_userSession.IsAuthenticated(user))
        {
            return;
        }

        UserDao dao = user.ToDao(_bitmapService);
        await _userRepository.UpdateAsync(dao);
    }
}
