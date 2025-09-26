namespace CarCare.Persistence.Interfaces.Private;

internal interface IMigrationsManager
{
    Task CreateMigrationHistoryTableAsync();

    Task UpdateToLatestAsync();
}
