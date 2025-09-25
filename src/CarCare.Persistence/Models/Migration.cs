namespace CarCare.Persistence.Models;

/// <summary>
/// A database migration model.
/// </summary>
public class Migration
{
    /// <summary>
    /// The id of the migration.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The version of the application that this migration is bound to.
    /// </summary>
    public string ProductVersion { get; set; }

    /// <summary>
    /// Creates a new <see cref="Migration"/> object.
    /// </summary>
    /// <param name="id">The id of the migration.</param>
    /// <param name="productVersion">The product version that the migration is bound to.</param>
    public Migration(string id, string productVersion)
    {
        Id = id;
        ProductVersion = productVersion;
    }
}
