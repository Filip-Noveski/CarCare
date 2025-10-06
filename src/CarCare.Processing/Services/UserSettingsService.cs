using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models.Core;

namespace CarCare.Processing.Services;

internal class UserSettingsService : IUserSettingsService
{
    private readonly IUserSettingsRepository _settingsRepository;

    public UserSettingsService(IUserSettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    public async Task AddAsync(UserSettings settings)
    {
        UserSettingsDao dao = settings.ToDao();
        await _settingsRepository.AddAsync(dao);
    }

    public async Task DeleteAsync(Guid userId)
    {
        await _settingsRepository.DeleteAsync(userId);
    }

    public async Task<UserSettings> GetAsync(Guid userId)
    {
        UserSettingsDao dao = await _settingsRepository.GetAsync(userId);
        UserSettings settings = new(dao.UserId);
        return settings;
    }

    public async Task UpdateAsync(UserSettings settings)
    {
        UserSettingsDao dao = settings.ToDao();
        await _settingsRepository.UpdateAsync(dao);
    }
}
