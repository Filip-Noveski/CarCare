namespace CarCare.Processing.Interfaces.Service;

internal interface IEventManagerService
{
    // raise when a car was added or deleted from MyCars
    event EventHandler MyCarsChanged;

    void OnMyCarsChanged();
}
