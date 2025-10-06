using CarCare.Persistence.Models;
using CarCare.Processing.Models.Dto;

namespace CarCare.Processing.Models.Core;

internal class UserSettings
{
    public Guid UserId { get; set; }

    public UserSettings(Guid userId)
    {
        UserId = userId;
    }

    public UserSettingsDto ToDto()
    {
        return new(UserId);
    }

    public UserSettingsDao ToDao()
    {
        return new(UserId);
    }

    public static UserSettings CreateDefault(Guid userId)
    {
        return new(userId);
    }
}
