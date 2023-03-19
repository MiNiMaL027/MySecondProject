using List_Service.Interfaces;

namespace List_Service.BackgroundTasks
{
    public class TaskRetentionPoleBackgroundTask
    {
        private readonly IToDoTaskService _taskService;

        public TaskRetentionPoleBackgroundTask(IToDoTaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task Run()
        {
            var tasks = await _taskService.GetAll();
            var ids = tasks.Where(x => x.ArchivalDate.Value.AddDays(30) >= DateTime.Now).Select(x => x.Id).ToList();

            await _taskService.RemoveFromDb(ids);
        }
    }
}
