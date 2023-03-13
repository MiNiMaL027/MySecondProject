using List_Domain.Models;

namespace List_Dal.Interfaces
{
    public interface ICustomListRepository : IDefaultRepository<CustomList>, IChekAuthorization<CustomList>
    {
        Task<bool> CheckIfNameExist(string name, int userId);
    }
}
