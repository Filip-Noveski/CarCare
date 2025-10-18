using System.Windows;
using System.Windows.Controls;

namespace CarCare.UserInterface.Controls;

/// <summary>
/// Interaction logic for TextBoxControl.xaml
/// </summary>
public partial class TextBoxControl : UserControl
{
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        name: nameof(Text),
        propertyType: typeof(string),
        ownerType: typeof(TextBoxControl));
    
    public static readonly DependencyProperty HeadingProperty = DependencyProperty.Register(
        name: nameof(Heading),
        propertyType: typeof(string),
        ownerType: typeof(TextBoxControl));

    /// <summary>
    /// The text contained in the inner text box.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// The heading/label of the text box.
    /// </summary>
    public string Heading
    {
        get => (string)GetValue(HeadingProperty);
        set => SetValue(HeadingProperty, value);
    }

    public TextBoxControl()
    {
        InitializeComponent();
    }
}
