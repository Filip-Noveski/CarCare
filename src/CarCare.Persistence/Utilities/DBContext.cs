using Microsoft.Extensions.Configuration;

namespace CarCare.Persistence.Utilities;

internal class DBContext
{
    public string ConnectionString { get; private set; }

    public DBContext(IConfiguration configuration)
    {
        string appName = configuration["Application:Name"]!;
        string dbName = configuration["Database:Name"]!;
        ConnectionString = CreateConnectionString(appName, dbName);
    }

    protected static string CreateConnectionString(string appName, string dbName)
    {
        Environment.SpecialFolder folderType = Environment.SpecialFolder.ApplicationData;
        string folderPath = Environment.GetFolderPath(folderType);
        string dbPath = Path.Combine(folderPath, appName, dbName);
        return $"Data Source={dbPath}";
    }
}
