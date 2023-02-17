using List_Domain.ViewModel;
using Microsoft.AspNetCore.Http;

namespace List_Service.Interfaces
{
    public interface ISettingsService
    {
        Task<ViewSettings> GetSettingsByUser();

        Task<int> RemoveSettings(int id);

        Task<int> CreateSettings(ViewSettings settings);

        Task<int> UpdateSetings(ViewSettings settings);
    }
}
