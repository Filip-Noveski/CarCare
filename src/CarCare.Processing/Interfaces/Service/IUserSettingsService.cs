using CarCare.Processing.Models.Core;

namespace CarCare.Processing.Interfaces.Service;

internal interface IUserSettingsService
{
    Task AddAsync(UserSettings settings);

    Task UpdateAsync(UserSettings settings);

    Task DeleteAsync(Guid userId);

    Task<UserSettings> GetAsync(Guid userId);
}
