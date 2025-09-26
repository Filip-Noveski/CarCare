using CarCare.Processing.Abstract;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;

namespace CarCare.Processing.Contexts;

internal class SplashScreenContext : Context, ISplashScreenContext
{
    private readonly IDatabaseManagementService _dbManagementService;

    public string OperationText 
    {
        get
        {
            return field;
        }
        private set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public SplashScreenContext(IDatabaseManagementService dbManagementService)
    {
        _dbManagementService = dbManagementService;
        OperationText = string.Empty;
    }

    public async Task InitialiseDatabaseAsync()
    {
        OperationText = "Verifying database existence and version...";
        await _dbManagementService.VerifyDatabaseStateAsync();
    }
}
