using System.Globalization;
using System.Windows.Data;

namespace CarCare.Processing.Converters;

/// <summary>
/// Converts an <see cref="Enum"/> to an <see cref="int"/>.
/// </summary>
public class EnumToIntConverter : IValueConverter
{
    /// <summary>
    /// Converts the <paramref name="value"/> into an <see cref="int"/>.
    /// </summary>
    /// <param name="value">The enum to convert.</param>
    /// <param name="targetType">Irrelevant.</param>
    /// <param name="parameter">Irrelevant.</param>
    /// <param name="culture">Irrelevant.</param>
    /// <returns>A <see cref="int"/> object.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value;
    }

    /// <summary>
    /// Converts the <paramref name="value"/> into the specified <paramref name="targetType"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">Irrelevant.</param>
    /// <param name="culture">Irrelevant.</param>
    /// <returns>An <see cref="Enum"/> of the specified <paramref name="targetType"/>.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Enum.ToObject(targetType, value);
    }
}
