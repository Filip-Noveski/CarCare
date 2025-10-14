using CarCare.Persistence.Models;
using CarCare.Processing.Enums;
using CarCare.Processing.Models.Dto;

namespace CarCare.Processing.Models.Core;

internal class UserSettings
{
    public Guid UserId { get; set; }

    public ApplicationTheme Theme { get; set; }

    public UserSettings(Guid userId, ApplicationTheme theme)
    {
        UserId = userId;
        Theme = theme;
    }

    public UserSettingsDto ToDto()
    {
        return new(UserId, Theme);
    }

    public UserSettingsDao ToDao()
    {
        return new(UserId, Theme.ToString());
    }

    public static UserSettings CreateDefault(Guid userId)
    {
        return new(userId, ApplicationTheme.Dark);
    }
}
