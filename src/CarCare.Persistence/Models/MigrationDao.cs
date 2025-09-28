namespace CarCare.Persistence.Models;

/// <summary>
/// A database migration model.
/// </summary>
public class MigrationDao
{
    /// <summary>
    /// The id of the migration.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Creates a new <see cref="MigrationDao"/> object.
    /// </summary>
    public MigrationDao()
    {
        Id = 0;
    }

    /// <summary>
    /// Creates a new <see cref="MigrationDao"/> object.
    /// </summary>
    /// <param name="id">The id of the migration.</param>
    public MigrationDao(int id)
    {
        Id = id;
    }
}
