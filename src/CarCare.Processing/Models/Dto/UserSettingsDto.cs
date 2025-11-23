using CarCare.Processing.Abstract;
using CarCare.Processing.Enums;

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
    /// The selected application theme.
    /// </summary>
    public ApplicationTheme Theme
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The preferred currency for financial data.
    /// </summary>
    public Currency PreferredCurrency
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
    /// <param name="theme">The selected application theme.</param>
    /// <param name="preferredCurrency">The preferred currency for financial data.</param>
    public UserSettingsDto(Guid userId, ApplicationTheme theme, Currency preferredCurrency)
    {
        UserId = userId;
        Theme = theme;
        PreferredCurrency = preferredCurrency;
    }
}
