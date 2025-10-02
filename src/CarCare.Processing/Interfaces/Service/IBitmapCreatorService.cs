using System.Windows.Media.Imaging;

namespace CarCare.Processing.Interfaces.Service;

internal interface IBitmapCreatorService
{
    BitmapImage GetGenericAvatar(string username);

    BitmapImage ConvertToBitmap(byte[] avatar);

    byte[] ConvertToBinary(BitmapImage image);
}
