using List_Domain.CreateModel;
using List_Domain.ModelDTO;

namespace List_Service.Interfaces
{
    public interface IToDoTaskService : IDefaultService<ToDoTaskDTO,CreateToDoTask>
    {
        Task<bool> CompleteTask(int id, int userId);
    }
}
