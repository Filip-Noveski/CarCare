namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// A data context for the Authentication window.
/// </summary>
public interface IAuthenticationContext
{
    /// <summary>
    /// An event where the Register view was requested.
    /// </summary>
    event EventHandler RegisterRequested;

    /// <summary>
    /// An event where the Login view was requested.
    /// </summary>
    event EventHandler LoginRequested;

    /// <summary>
    /// Specifies that the Register view was requested.
    /// </summary>
    void OnRegisterRequested();

    /// <summary>
    /// Specifies that the Login view was requested.
    /// </summary>
    void OnLoginRequested();
}
