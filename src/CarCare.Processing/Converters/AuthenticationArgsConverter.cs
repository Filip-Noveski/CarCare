using CarCare.Processing.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CarCare.Processing.Converters;

public class AuthenticationArgsConverter : IMultiValueConverter
{
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

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
