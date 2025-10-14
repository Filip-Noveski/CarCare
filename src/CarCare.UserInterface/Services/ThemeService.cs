using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Service;
using System.Diagnostics;
using System.Windows;

namespace CarCare.UserInterface.Services;

internal class ThemeService : IThemeService
{
    public ApplicationTheme Theme { get; private set; }

    public void ChangeTheme(ApplicationTheme theme)
    {
        Theme = theme;
        ResourceDictionary next = new()
        {
            Source = theme switch
            {
                ApplicationTheme.Light => new("./Themes/LightTemplate.xaml"),
                ApplicationTheme.Dark => new("./Themes/DarkTemplate.xaml"),
                _ => throw new UnreachableException()
            }
        };

        ResourceDictionary resources = Application.Current.Resources;
        ResourceDictionary? previous = resources.MergedDictionaries
            .FirstOrDefault(x => x.Source is not null && x.Source.OriginalString.Contains("Template.xaml"));
        if (previous is not null)
        {
            resources.MergedDictionaries.Remove(previous);
        }
        resources.MergedDictionaries.Add(next);
    }

    public void SetDefaultTheme()
    {
        // default to Dark
        ChangeTheme(ApplicationTheme.Dark);
    }
}
