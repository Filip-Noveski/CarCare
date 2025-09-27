namespace CarCare.Persistence.Models;

/// <summary>
/// A database migration model.
/// </summary>
public class Migration
{
    /// <summary>
    /// The id of the migration.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Creates a new <see cref="Migration"/> object.
    /// </summary>
    public Migration()
    {
        Id = 0;
    }

    /// <summary>
    /// Creates a new <see cref="Migration"/> object.
    /// </summary>
    /// <param name="id">The id of the migration.</param>
    public Migration(int id)
    {
        Id = id;
    }
}
