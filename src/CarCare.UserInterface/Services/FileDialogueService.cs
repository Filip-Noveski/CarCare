using CarCare.Processing.Interfaces.Service;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace CarCare.UserInterface.Services;

internal class FileDialogueService : IFileDialogueService
{
    public BitmapImage? GetImageFile()
    {
        OpenFileDialog dialogue = new()
        {
            Filter = "Image Files (*.png;*jpg;*.jpeg;*.bmp)|*.png;*jpg;*.jpeg;*.bmp"
        };

        if (dialogue.ShowDialog() is true)
        {
            return new(new Uri(dialogue.FileName));
        }

        return null;
    }
}
