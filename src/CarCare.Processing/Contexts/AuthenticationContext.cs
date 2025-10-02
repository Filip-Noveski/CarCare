using CarCare.Processing.Interfaces.Context;

namespace CarCare.Processing.Contexts;

internal class AuthenticationContext : IAuthenticationContext
{
    public event EventHandler RegisterRequested = null!;

    public event EventHandler LoginRequested = null!;

    public AuthenticationContext()
    {
    }

    public void OnRegisterRequested()
    {
        RegisterRequested?.Invoke(this, EventArgs.Empty);
    }

    public void OnLoginRequested()
    {
        LoginRequested?.Invoke(this, EventArgs.Empty);
    }
}
