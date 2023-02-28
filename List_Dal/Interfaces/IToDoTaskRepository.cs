using List_Domain.Models;

namespace List_Dal.Interfaces
{
    public interface IToDoTaskRepository : IDefaultRepository<ToDoTask>
    {
        Task<bool> CheckIfNameExist(string title, int uresId);

        Task<bool> CompleteTask(int id);

        Task<List<ToDoTask>> GetByListName(string listName, int userId);
    }
}
