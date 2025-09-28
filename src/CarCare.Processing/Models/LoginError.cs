namespace CarCare.Processing.Models;

/// <summary>
/// Represents an error from a failed login attempt.
/// </summary>
/// <param name="Message">The error message to display.</param>
public record struct LoginError(string Message);
