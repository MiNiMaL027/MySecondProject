using List_Domain.Models;

namespace List_Service.Interfaces
{
    public interface IToDoTaskService : IDefaultService<ToDoTaskDTO,CreateToDoTask>
    {
        Task<bool> CompleteTask(int id, int userId);
    }
}
