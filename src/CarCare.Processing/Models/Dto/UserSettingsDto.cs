using CarCare.Processing.Abstract;

namespace CarCare.Processing.Models.Dto;

/// <summary>
/// A transferable User Settings model.
/// </summary>
public class UserSettingsDto : Context
{
    /// <summary>
    /// The id of the user that owns the settings.
    /// </summary>
    public Guid UserId
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserSettingsDto"/> class.
    /// </summary>
    /// <param name="userId">The id of the user that owns the settings.</param>
    public UserSettingsDto(Guid userId)
    {
        UserId = userId;
    }
}
