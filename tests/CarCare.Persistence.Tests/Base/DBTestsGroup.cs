using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace CarCare.Persistence.Tests.Base;

public abstract class DBTestsGroup : IAsyncLifetime
{
    private const int RetryCount = 10;

    protected IConfiguration Configuration { get; }

    protected ITestOutputHelper Output { get; }

    protected DBTestsGroup(ITestOutputHelper output)
    {
        Output = output;
        ConfigurationBuilder builder = new();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");

        Configuration = builder.Build();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Environment.SpecialFolder folderType = Environment.SpecialFolder.ApplicationData;
        string folderPath = Environment.GetFolderPath(folderType);

        string appName = Configuration["Application:Name"]!;
        string dbName = Configuration["Database:Name"]!;
        string dbPath = Path.Combine(folderPath, appName, dbName);

        SqliteConnection.ClearAllPools();

        if (File.Exists(dbPath))
        {
            await DeleteFile(dbPath);
        }
    }

    private async Task DeleteFile(string path)
    {
        int remainingRetries = RetryCount;
        do
        {
            try
            {
                File.Delete(path);
                Output.WriteLine($"Deletion retries: {RetryCount - remainingRetries}");
                return;
            }
            catch (IOException)
            {
                remainingRetries--;
                // wait progressively more
                await Task.Delay((RetryCount - remainingRetries) * 100);
            }
        } while (remainingRetries > 0);

        Output.WriteLine("Failed to delete DB file");
    }
}
