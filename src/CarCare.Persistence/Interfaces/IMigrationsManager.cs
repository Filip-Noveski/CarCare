namespace CarCare.Persistence.Interfaces;

internal interface IMigrationsManager
{
    Task CreateMigrationHistoryTableAsync();

    Task UpdateToLatestAsync();
}
