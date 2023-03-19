using List_Domain.Models;

namespace List_Service.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);

        Task<User> GetByEmail(string email);

        IQueryable<User> GetAll();

        Task<bool> RestoreUser(string email);

        Task<bool> RemoveUser(int id);

        Task<bool> RemoveUserFromDb(List<int> ids);
    }
}
