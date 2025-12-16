using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarCare.UserInterface.Controls;

/// <summary>
/// Interaction logic for ComboBoxControl.xaml
/// </summary>
public partial class ComboBoxControl : UserControl
{
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        name: nameof(SelectedItem),
        propertyType: typeof(object),
        ownerType: typeof(ComboBoxControl));

    public static readonly DependencyProperty HeadingProperty = DependencyProperty.Register(
        name: nameof(Heading),
        propertyType: typeof(string),
        ownerType: typeof(ComboBoxControl));

    public static readonly DependencyProperty SelectionChangedCommandProperty = DependencyProperty.Register(
        name: nameof(SelectionChangedCommand),
        propertyType: typeof(ICommand),
        ownerType: typeof(ComboBoxControl));

    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        name: nameof(ItemsSource),
        propertyType: typeof(IEnumerable),
        ownerType: typeof(ComboBoxControl));

    /// <summary>
    /// The selected item of the inner combo box.
    /// </summary>
    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    /// The heading/label of the combo box.
    /// </summary>
    public string Heading
    {
        get => (string)GetValue(HeadingProperty);
        set => SetValue(HeadingProperty, value);
    }

    /// <summary>
    /// The command invoked when the selection is changed.
    /// </summary>
    public ICommand SelectionChangedCommand
    {
        get => (ICommand)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }

    /// <summary>
    /// The source of items.
    /// </summary>
    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public ComboBoxControl()
    {
        InitializeComponent();
    }

    private void SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectionChangedCommand?.Execute(null);
    }
}
