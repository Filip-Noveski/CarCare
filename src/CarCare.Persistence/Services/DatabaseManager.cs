using CarCare.Persistence.Configuration;
using CarCare.Persistence.Interfaces;

namespace CarCare.Persistence.Services;

internal class DatabaseManager : IDatabaseManager
{
    private readonly IMigrationsHistoryRepository _historyRepository;
    private readonly MigrationsManager _migrationsManager;

    public DatabaseManager(IMigrationsHistoryRepository historyRepository, MigrationsManager migrationsManager)
    {
        _historyRepository = historyRepository;
        _migrationsManager = migrationsManager;
    }

    public async Task VerifyDatabaseStateAsync()
    {
        if (!await _historyRepository.ExistsAsync())
        {
            await _migrationsManager.CreateMigrationHistoryTableAsync();
        }

        await _migrationsManager.UpdateToLatestAsync();
        throw new NotImplementedException();
    }
}
