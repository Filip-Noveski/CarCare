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
    /// Creates a new instance of the <see cref="UserSettingsDao"/> class.
    /// </summary>
    public UserSettingsDao()
    {
        UserId = Guid.Empty;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="UserSettingsDao"/> class.
    /// </summary>
    /// <param name="userId">The id of the user that owns the settings.</param>
    public UserSettingsDao(Guid userId)
    {
        UserId = userId;        
    }
}
