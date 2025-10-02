using CarCare.Processing.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CarCare.Processing.Converters;

/// <summary>
/// Converts an array into <see cref="AuthenticationArgs"/>.
/// </summary>
public class AuthenticationArgsConverter : IMultiValueConverter
{
    /// <summary>
    /// Converts the <paramref name="values"/> into an <see cref="AuthenticationArgs"/> object.
    /// </summary>
    /// <param name="values">The properties to append.</param>
    /// <param name="targetType">Irrelevant.</param>
    /// <param name="parameter">Irrelevant.</param>
    /// <param name="culture">Irrelevant.</param>
    /// <returns>A <see cref="AuthenticationArgs"/> object.</returns>
    /// <exception cref="ArgumentException"></exception>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return new AuthenticationArgs
        {
            Window = values[0] as Window 
                ?? throw new ArgumentException("Value 0 must be of type Window"),
            PasswordBox = values[1] as PasswordBox 
                ?? throw new ArgumentException("Value 1 must be of type PasswordBox")
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
