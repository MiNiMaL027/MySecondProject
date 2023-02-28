using List_Domain.Models;

namespace List_Dal.Interfaces
{
    public interface ISettingsRepository
    {
        Task<Settings> GetSettingsByUser(int userId);

        Task<int> RemoveSettings(int id);

        Task<int> CreateSettings(Settings settings);

        Task<bool> UpdateSetings(Settings settings);
    };
}
