using CarCare.Processing.Abstract;
using CarCare.Processing.Commands;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Communication;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Models.Dto;
using Microsoft.AspNetCore.Identity;
using OneOf;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Contexts;

internal class AccountSettingsContext : Context, IAccountSettingsContext
{
    private readonly IUserService _userService;
    private readonly IUserSession _userSession;
    private readonly IPasswordHasher<User> _hasher;
    private readonly IFileDialogueService _fileService;
    private readonly IBitmapCreatorService _bitmapCreator;

    public string Username { get; set; } = string.Empty;

    public string UsernameChangeError 
    { 
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    } = string.Empty;

    public string PasswordChangeError 
    { 
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    } = string.Empty;

    public string UsernameChangeSuccess
    { 
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    } = string.Empty;

    public string PasswordChangeSuccess
    { 
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    } = string.Empty;

    public string AvatarChangeSuccess
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    } = string.Empty;

    public BitmapImage? Avatar
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public ICommand UpdateUsernameCommand { get; }

    public ICommand UpdatePasswordCommand { get; }

    public ICommand UpdateAvatarCommand { get; }

    public ICommand ChooseAvatarCommand { get; }

    public ICommand DeleteAvatarCommand { get; }

    public AccountSettingsContext(
        IUserService userService,
        IUserSession userSession,
        IPasswordHasher<User> hasher,
        IFileDialogueService fileService,
        IBitmapCreatorService bitmapCreator)
    {
        _userService = userService;
        _userSession = userSession;
        _hasher = hasher;
        _fileService = fileService;
        _bitmapCreator = bitmapCreator;
        Avatar = _userSession.User!.Avatar;
        UpdateUsernameCommand = new Command(UpdateUsername);
        UpdatePasswordCommand = new Command(UpdatePassword);
        UpdateAvatarCommand = new Command(UpdateAvatar);
        ChooseAvatarCommand = new Command(ChooseAvatar);
        DeleteAvatarCommand = new Command(DeleteAvatar);
    }

    private async void UpdateUsername(object? parameter)
    {
        UserDto? dto = _userSession.User;
        if (dto is null)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(Username))
        {
            UsernameChangeSuccess = string.Empty;
            UsernameChangeError = "The username cannot be empty or whitespace";
            return;
        }
        
        // TODO: change to GetAsync
        User user = await _userService.GetUserAsync(dto.Id);
        user.Username = Username;
        if (!user.HasCustomAvatar)
        {
            user.Avatar = _bitmapCreator.GetGenericAvatar(Username);
        }
        await _userService.UpdateAsync(user);
        dto.Username = Username;
        UsernameChangeError = string.Empty;
        UsernameChangeSuccess = $"The username was successfully updated to '{Username}'";
    }

    private async void UpdatePassword(object? parameter)
    {
        UserDto? dto = _userSession.User;
        if (dto is null) 
        { 
            return;
        }
        if (parameter is not UpdatePasswordArgs args)
        {
            return;
        }

        string oldPass = args.OldPasswordBox.Password;
        OneOf<User, LoginError> result = await _userService.LoginAsync(dto.Username, oldPass);
        if (result.Value is LoginError error)
        {
            PasswordChangeSuccess = string.Empty;
            PasswordChangeError = error.Message;
            return;
        }

        string newPass = args.NewPasswordBox.Password;
        string confPass = args.ConfPasswordBox.Password;
        if (string.IsNullOrWhiteSpace(newPass))
        {
            PasswordChangeError = "The password cannot be empty or whitespace";
            return;
        }
        if (newPass != confPass)
        {
            PasswordChangeError = "Please confirm the new password correctly";
            return;
        }

        User user = await _userService.GetUserAsync(dto.Id);
        user.Password = _hasher.HashPassword(user, newPass);
        await _userService.UpdateAsync(user);
        PasswordChangeError = string.Empty;
        PasswordChangeSuccess = "The password was successfully updated";
    }

    private async void UpdateAvatar(object? parameter)
    {
        UserDto? dto = _userSession.User;
        if (dto is null)
        {
            return;
        }

        User user = await _userService.GetUserAsync(dto.Id);
        user.Avatar = Avatar ?? _bitmapCreator.GetGenericAvatar(dto.Username);
        user.HasCustomAvatar = Avatar is not null;
        await _userService.UpdateAsync(user);
        _userSession.User!.Avatar = user.Avatar;
        AvatarChangeSuccess = "The avatar was updated successfully";
    }

    private void ChooseAvatar(object? parameter)
    {
        Avatar = _fileService.GetImageFile();
    }

    private void DeleteAvatar(object? parameter)
    {
        Avatar = null;
    }
}
