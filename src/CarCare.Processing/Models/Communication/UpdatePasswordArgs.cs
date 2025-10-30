using System.Windows.Controls;

namespace CarCare.Processing.Models.Communication;

/// <summary>
/// A model containing the arguments for user password updating.
/// </summary>
public class UpdatePasswordArgs
{
    /// <summary>
    /// The password box containing the old password.
    /// </summary>
    public PasswordBox OldPasswordBox { get; set; } = null!;

    /// <summary>
    /// The password box containing the new password.
    /// </summary>
    public PasswordBox NewPasswordBox { get; set; } = null!;

    /// <summary>
    /// The password box containing the new password for confirmation.
    /// </summary>
    public PasswordBox ConfPasswordBox { get; set; } = null!;
}
