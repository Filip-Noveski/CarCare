namespace CarCare.Persistence.Models;

/// <summary>
/// A database user settings model.
/// </summary>
public class UserSettingsDao
{
    /// <summary>
    /// The id of the user that owns the settings.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The name of the application theme the user has selected.
    /// </summary>
    public string Theme { get; set; }

    /// <summary>
    /// The user's preferred currency for cost presentation.
    /// </summary>
    public string PreferredCurrency { get; set; }

    /// <summary>
    /// Creates a new instance of the <see cref="UserSettingsDao"/> class.
    /// </summary>
    public UserSettingsDao()
    {
        UserId = Guid.Empty;
        Theme = string.Empty;
        PreferredCurrency = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserSettingsDao"/> class.
    /// </summary>
    /// <param name="userId">The id of the user that owns the settings.</param>
    /// <param name="theme">The name of the user's selected theme.</param>
    /// <param name="preferredCurrency">The preferred display currency.</param>
    public UserSettingsDao(Guid userId, string theme, string preferredCurrency)
    {
        UserId = userId;
        Theme = theme;
        PreferredCurrency = preferredCurrency;
    }
}
