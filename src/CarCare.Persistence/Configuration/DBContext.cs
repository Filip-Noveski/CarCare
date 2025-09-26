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
        _connectionString = CreateConnectionString(appName, dbName);
    }

    protected static string CreateConnectionString(string appName, string dbName)
    {
        Environment.SpecialFolder folderType = Environment.SpecialFolder.ApplicationData;
        string folderPath = Environment.GetFolderPath(folderType);
        string dbPath = Path.Combine(folderPath, appName, dbName);
        return $"Data Source={dbPath}";
    }

    public async Task<SqliteConnection> CreateConnection()
    {
        SqliteConnection connection = new(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
