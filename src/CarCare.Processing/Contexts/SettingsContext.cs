using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Context;

namespace CarCare.Processing.Contexts;

internal class SettingsContext : WindowContext, ISettingsContext
{
    public SettingsTab Tab 
    { 
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }
}
