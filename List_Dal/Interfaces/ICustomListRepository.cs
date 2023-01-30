using List_Domain.Models;

namespace List_Dal.Interfaces
{
    public interface ICustomListRepository : IDefaultRepository<CustomList>
    {
        Task<bool> CheckIfNameExist(string name, int userId);
    }
}
