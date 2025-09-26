using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CarCare.Persistence.Configuration;

internal class MigrationsManager : IMigrationsManager
{
    private readonly DBContext _context;
    private readonly IMigrationsHistoryRepository _historyRepository;
    private readonly Dictionary<int, string> _migrations;

    public MigrationsManager(
        IConfiguration configuration,
        DBContext context,
        IMigrationsHistoryRepository historyRepository)
    {
        CreateAppDataDirectory(configuration);
        _context = context;
        _historyRepository = historyRepository;
        _migrations = GetMigrations();
    }

    private static void CreateAppDataDirectory(IConfiguration configuration)
    {
        string appName = configuration["Application:Name"]!;
        Environment.SpecialFolder appDataType = Environment.SpecialFolder.ApplicationData;
        string appDataPath = Environment.GetFolderPath(appDataType);
        string finalPath = Path.Combine(appDataPath, appName);
        if (!Directory.Exists(finalPath))
        {
            Directory.CreateDirectory(finalPath);
        }
    }

    private static Dictionary<int, string> GetMigrations()
    {
        string[] files = Directory.GetFiles("./Migrations");
        Dictionary<int, string> dictionary = new(files.Length);
        foreach (string file in files)
        {
            int start = file.IndexOf('.', 1) + 1;
            int end = file.LastIndexOf('.');
            ReadOnlySpan<char> idStr = file.AsSpan()[start..end];
            int id = int.Parse(idStr);
            dictionary.Add(id, file);
        }
        return dictionary;
    }

    public async Task CreateMigrationHistoryTableAsync()
    {
        (int id, string path) = _migrations.First();
        string sql = File.ReadAllText(path);
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        await connection.ExecuteAsync(sql);
        await _historyRepository.AddAsync(new Migration(id));
    }

    public async Task UpdateToLatestAsync()
    {
        Migration latest = await _historyRepository.GetLatestMigrationAsync();
        int id = latest.Id;
        using SqliteConnection connection = await _context.CreateConnectionAsync();
        foreach ((int key, string file) in _migrations)
        {
            if (key <= id)
            {
                continue;
            }

            string sql = File.ReadAllText(file);
            await connection.ExecuteAsync(sql);
            await _historyRepository.AddAsync(new Migration(key));
        }
    }
}
