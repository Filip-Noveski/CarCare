using CarCare.Persistence.Interfaces;
using CarCare.Processing.Interfaces.Service;

namespace CarCare.Processing.Services;

internal class DatabaseManagementService : IDatabaseManagementService
{
    private readonly IDatabaseManager _manager;

    public DatabaseManagementService(IDatabaseManager manager)
    {
        _manager = manager;
    }

    public async Task VerifyDatabaseStateAsync()
    {
        await _manager.VerifyDatabaseStateAsync();
    }
}
