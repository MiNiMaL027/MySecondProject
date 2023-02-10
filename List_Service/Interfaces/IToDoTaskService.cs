using List_Domain.CreateModel;
using List_Domain.ViewModel;

namespace List_Service.Interfaces
{
    public interface IToDoTaskService : IDefaultService<ViewToDoTask,CreateToDoTask>
    {
        Task<bool> CompleteTask(int id);
    }
}
