using CarCare.Processing.Interfaces.Service;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Services;

internal class BitmapCreatorService : IBitmapCreatorService
{
    private static readonly Color GenericAvatarBackground = Color.FromArgb(255, 255, 32, 32);
    private static readonly Color GenericAvatarForeground = Color.FromArgb(255, 32, 16, 16);
    private const int GenericAvatarSize = 256;
    private const int GenericAvatarFontSize = 256 * 6 / 10;

    public BitmapImage ConvertToBitmap(byte[] avatar)
    {
        BitmapImage image = new();

        using MemoryStream stream = new(avatar);
        stream.Position = 0;
        image.BeginInit();
        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.UriSource = null;
        image.StreamSource = stream;
        image.EndInit();
        image.Freeze();

        return image;
    }

    public byte[] ConvertToBinary(BitmapImage image)
    {
        byte[] data;
        PngBitmapEncoder encoder = new();
        encoder.Frames.Add(BitmapFrame.Create(image));

        using MemoryStream stream = new();
        encoder.Save(stream);
        data = stream.ToArray();

        return data;
    }

    public BitmapImage GetGenericAvatar(string username)
    {
        string letter = username[0].ToString();
        RenderTargetBitmap bmp = DrawGenericAvatar(letter);
        return CreateImage(bmp);
    }

    private static RenderTargetBitmap DrawGenericAvatar(string letter)
    {
        SolidColorBrush background = new(GenericAvatarBackground);
        SolidColorBrush foreground = new(GenericAvatarForeground);

        DrawingVisual visual = new();
        using (DrawingContext context = visual.RenderOpen())
        {
            context.DrawRectangle(background, null, new Rect(0, 0, GenericAvatarSize, GenericAvatarSize));
            FormattedText formattedText = new(
                letter,
                CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Bahnschrift"),
                GenericAvatarFontSize,
                foreground,
                VisualTreeHelper.GetDpi(visual).PixelsPerDip);

            double textX = (GenericAvatarSize - formattedText.Width) / 2 + 2;
            double textY = (GenericAvatarSize - formattedText.Height) / 2 + 6;

            context.DrawText(formattedText, new(textX, textY));
        }

        RenderTargetBitmap bmp = new(GenericAvatarSize, GenericAvatarSize, 96, 96, PixelFormats.Pbgra32);
        bmp.Render(visual);

        return bmp;
    }

    private static BitmapImage CreateImage(RenderTargetBitmap bmp)
    {
        PngBitmapEncoder encoder = new();
        encoder.Frames.Add(BitmapFrame.Create(bmp));

        using MemoryStream stream = new();
        encoder.Save(stream);
        stream.Position = 0;

        BitmapImage image = new();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = stream;
        image.EndInit();
        image.Freeze();

        return image;
    }
}
