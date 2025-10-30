using CarCare.Processing.Models.Communication;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace CarCare.Processing.Converters;

/// <summary>
/// Converts an array into <see cref="UpdatePasswordArgs"/>.
/// </summary>
public class UpdatePasswordArgsConverter : IMultiValueConverter
{
    /// <summary>
    /// Converts the <paramref name="values"/> into an <see cref="UpdatePasswordArgs"/> object.
    /// </summary>
    /// <param name="values">The properties to append.</param>
    /// <param name="targetType">Irrelevant.</param>
    /// <param name="parameter">Irrelevant.</param>
    /// <param name="culture">Irrelevant.</param>
    /// <returns>A <see cref="UpdatePasswordArgs"/> object.</returns>
    /// <exception cref="ArgumentException"></exception>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return new UpdatePasswordArgs
        {
            OldPasswordBox = values[0] as PasswordBox
                ?? throw new ArgumentException("Value 0 must be of type PasswordBox"),
            NewPasswordBox = values[1] as PasswordBox
                ?? throw new ArgumentException("Value 1 must be of type PasswordBox"),
            ConfPasswordBox = values[2] as PasswordBox
                ?? throw new ArgumentException("Value 2 must be of type PasswordBox")
        };
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    /// <param name="value">Irrelevant.</param>
    /// <param name="targetTypes">Irrelevant.</param>
    /// <param name="parameter">Irrelevant.</param>
    /// <param name="culture">Irrelevant.</param>
    /// <returns>Irrelevant.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
