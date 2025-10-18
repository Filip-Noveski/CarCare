using System.Windows;
using System.Windows.Controls;

namespace CarCare.UserInterface.Controls;

/// <summary>
/// Interaction logic for TextBoxControl.xaml
/// </summary>
public partial class PasswordBoxControl : UserControl
{
    public static readonly DependencyProperty HeadingProperty = DependencyProperty.Register(
        name: nameof(Heading),
        propertyType: typeof(string),
        ownerType: typeof(PasswordBoxControl));

    /// <summary>
    /// The heading/label of the text box.
    /// </summary>
    public string Heading
    {
        get => (string)GetValue(HeadingProperty);
        set => SetValue(HeadingProperty, value);
    }

    /// <summary>
    /// Returns the inner password box for binding.
    /// </summary>
    public PasswordBox InnerPasswordBox => PasswordBox;

    public PasswordBoxControl()
    {
        InitializeComponent();
    }
}
