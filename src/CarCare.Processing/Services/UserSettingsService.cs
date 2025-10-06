using CarCare.Persistence.Interfaces;
using CarCare.Persistence.Models;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;

namespace CarCare.Processing.Services;

internal class UserSettingsService : IUserSettingsService
{
    private readonly IUserSettingsRepository _settingsRepository;
    private readonly IUserSession _session;

    public UserSettingsService(IUserSettingsRepository settingsRepository, IUserSession session)
    {
        _settingsRepository = settingsRepository;
        _session = session;
    }

    public async Task AddAsync(UserSettings settings)
    {
        UserSettingsDao dao = settings.ToDao();
        await _settingsRepository.AddAsync(dao);
    }

    public async Task DeleteAsync(Guid userId)
    {
        if (!_session.IsAuthenticated(userId))
        {
            return;
        }

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
        if (!_session.IsAuthenticated(settings.UserId))
        {
            return;
        }

        UserSettingsDao dao = settings.ToDao();
        await _settingsRepository.UpdateAsync(dao);
    }
}
