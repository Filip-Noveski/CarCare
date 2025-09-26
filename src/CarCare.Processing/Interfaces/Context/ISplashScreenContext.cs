namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// Data context for the splash screen of the application.
/// </summary>
public interface ISplashScreenContext
{
    /// <summary>
    /// The text that is displayed to inform the user of the pending operation.
    /// </summary>
    string OperationText { get; }

    /// <summary>
    /// Verifies the database existence and validity, and creates/updates it if necessary.
    /// </summary>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    Task InitialiseDatabaseAsync();
}
