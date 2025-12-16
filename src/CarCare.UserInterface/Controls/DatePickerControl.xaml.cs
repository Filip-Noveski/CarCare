using System.Windows;
using System.Windows.Controls;

namespace CarCare.UserInterface.Controls;

/// <summary>
/// Interaction logic for DatePickerControl.xaml
/// </summary>
public partial class DatePickerControl : UserControl
{
    public static readonly DependencyProperty DateProperty = DependencyProperty.Register(
        name: nameof(Date),
        propertyType: typeof(DateTime),
        ownerType: typeof(DatePickerControl));

    public static readonly DependencyProperty HeadingProperty = DependencyProperty.Register(
        name: nameof(Heading),
        propertyType: typeof(string),
        ownerType: typeof(DatePickerControl));

    /// <summary>
    /// The date contained in the inner date picker.
    /// </summary>
    public DateTime Date
    {
        get => (DateTime)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }

    /// <summary>
    /// The heading/label of the date picker.
    /// </summary>
    public string Heading
    {
        get => (string)GetValue(HeadingProperty);
        set => SetValue(HeadingProperty, value);
    }

    public DatePickerControl()
    {
        InitializeComponent();
    }
}
