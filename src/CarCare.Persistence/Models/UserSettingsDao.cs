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
}
