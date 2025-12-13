using CarCare.Processing.Interfaces.Service;

namespace CarCare.Processing.Services;

internal class EventManagerService : IEventManagerService
{
    public event EventHandler MyCarsChanged;

    public EventManagerService()
    {
        MyCarsChanged += NullEventHandler;
    }

    private void NullEventHandler(object? sender, EventArgs e) { }

    public void OnMyCarsChanged()
    {
        MyCarsChanged(this, EventArgs.Empty);
    }
}
