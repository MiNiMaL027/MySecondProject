using List_Domain.CreateModel;
using List_Domain.Models;
using List_Domain.ViewModel;

namespace List_Service.Interfaces
{
    public interface IToDoTaskService : IDefaultService<ViewToDoTask,CreateToDoTask>
    {
        Task<bool> CompleteTask(int id);

        Task<IQueryable<ViewToDoTask>> GetByListName(string listName);

        Task<IQueryable<ViewToDoTask>> GetByBaseList(int baseListId);
    }
}
