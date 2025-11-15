using System.Windows;

namespace CarCare.UserInterface.Services;

internal static class ResourceRegistrationService
{
    private const string TemplatePack = "pack://application:,,,/CarCare.UserInterface;component";

    public static void RegisterResources()
    {
        List<ResourceDictionary> dictionaries = new();
        AddStylesTo(dictionaries);

        ResourceDictionary resources = Application.Current.Resources;
        foreach (ResourceDictionary dictionary in dictionaries)
        {
            resources.MergedDictionaries.Add(dictionary);
        }
    }

    private static void AddStylesTo(List<ResourceDictionary> dictionaries)
    {
        dictionaries.Add(GetStyle("Button"));
        dictionaries.Add(GetStyle("ButtonCheckBox"));
        dictionaries.Add(GetStyle("CheckBox"));
        dictionaries.Add(GetStyle("ComboBox"));
        dictionaries.Add(GetStyle("ContextMenu"));
        dictionaries.Add(GetStyle("ContextMenuItem"));
        dictionaries.Add(GetStyle("DownwardExpander"));
        dictionaries.Add(GetStyle("IconButton"));
        dictionaries.Add(GetStyle("InputControlBorder"));
        dictionaries.Add(GetStyle("InputControlComboBox"));
        dictionaries.Add(GetStyle("InputControlPasswordBox"));
        dictionaries.Add(GetStyle("InputControlTextBox"));
        dictionaries.Add(GetStyle("Menu"));
        dictionaries.Add(GetStyle("MenuItem"));
        dictionaries.Add(GetStyle("PasswordBox"));
        dictionaries.Add(GetStyle("Separator"));
        dictionaries.Add(GetStyle("Slider"));
        dictionaries.Add(GetStyle("TabControl"));
        dictionaries.Add(GetStyle("TabItem"));
        dictionaries.Add(GetStyle("TextBlocks"));
        dictionaries.Add(GetStyle("TextBox"));
        dictionaries.Add(GetStyle("VerticalStackTabControl"));
        dictionaries.Add(GetStyle("VerticalStackTabItem"));
        dictionaries.Add(GetStyle("WindowCloseButton"));
        dictionaries.Add(GetStyle("WindowControlButton"));
    }

    private static ResourceDictionary GetStyle(string name)
    {
        return new()
        {
            Source = new($"{TemplatePack}/Styles/{name}.xaml")
        };
    }
}
