using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CarCare.Persistence.Configuration;

internal class DBContext
{
    private readonly string _connectionString;

    public DBContext(IConfiguration configuration)
    {
        string appName = configuration["Application:Name"]!;
        string dbName = configuration["Database:Name"]!;
        string dbOptions = configuration["Database:Options"] ?? string.Empty;
        _connectionString = CreateConnectionString(appName, dbName, dbOptions);
    }

    protected static string CreateConnectionString(string appName, string dbName, string options)
    {
        Environment.SpecialFolder folderType = Environment.SpecialFolder.ApplicationData;
        string folderPath = Environment.GetFolderPath(folderType);
        string dbPath = Path.Combine(folderPath, appName, dbName);
        return $"Data Source={dbPath};{options}";
    }

    public async Task<SqliteConnection> CreateConnection()
    {
        SqliteConnection connection = new(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
