using System.Windows.Media.Imaging;

namespace CarCare.Processing.Interfaces.Service;

/// <summary>
/// A service for dealing with file dialogues.
/// </summary>
public interface IFileDialogueService
{
    /// <summary>
    /// Opens a dialogue to select and attach a desired image.
    /// </summary>
    /// <returns>A <see cref="BitmapImage"/> or null.</returns>
    BitmapImage? GetImageFile();
}
