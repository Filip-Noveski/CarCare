using CarCare.Persistence.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;
using Xunit.Abstractions;

namespace CarCare.Persistence.Tests.Base;
public abstract class RepositoryTestsGroup : DBTestsGroup
{
    protected abstract string RelevantTablesSql { get; }

    internal DBContext Context { get; }

    public RepositoryTestsGroup(ITestOutputHelper output) : base(output)
    {
        Context = new(Configuration);
    }

    public override async Task InitializeAsync()
    {
        using SqliteConnection connection = await Context.CreateConnectionAsync();
        await connection.ExecuteAsync(RelevantTablesSql);

        await base.InitializeAsync();
    }
}
