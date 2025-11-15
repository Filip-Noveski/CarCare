using CarCare.Processing.Contexts;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Services;
using CarCare.Processing.Sessions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.Processing.Extensions;

/// <summary>
/// Provides extensions to the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Processing level services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static void AddProcessingServices(this IServiceCollection services)
    {
        services.AddSingleton<IDatabaseManagementService, DatabaseManagementService>();
        services.AddScoped<IWindowContext, WindowContext>();
        services.AddScoped<ISplashScreenContext, SplashScreenContext>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddSingleton<IUserSession, UserSession>();
        services.AddSingleton<IUserService, UserService>();
        services.AddScoped<IAuthenticationContext, AuthenticationContext>();
        services.AddScoped<IRegisterContext, RegisterContext>();
        services.AddScoped<ILoginContext, LoginContext>();
        services.AddSingleton<IBitmapCreatorService, BitmapCreatorService>();
        services.AddTransient<ISessionControlContext, SessionControlContext>();
        services.AddSingleton<IUserSettingsService, UserSettingsService>();

        // to allow for tab changing; proper data should be in specific data contexts bound to tabs
        services.AddSingleton<ISettingsContext, SettingsContext>();

        services.AddScoped<IAccountSettingsContext, AccountSettingsContext>();
        services.AddScoped<IDashboardContext, DashboardContext>();
    }
}
