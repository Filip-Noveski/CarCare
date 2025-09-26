using CarCare.Persistence.Interfaces;

namespace CarCare.Persistence.Services;

internal class DatabaseManager : IDatabaseManager
{
    private readonly IMigrationsHistoryRepository _historyRepository;
    private readonly IMigrationsManager _migrationsManager;

    public DatabaseManager(IMigrationsHistoryRepository historyRepository, IMigrationsManager migrationsManager)
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
    }
}
