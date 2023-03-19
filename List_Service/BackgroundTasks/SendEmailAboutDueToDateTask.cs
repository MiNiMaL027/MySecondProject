using List_Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace List_Service.BackgroundTasks
{
    public class SendEmailAboutDueToDateTask
    {
        private readonly IUserService _userService;
        private readonly IToDoTaskService _todoTaskService;

        public SendEmailAboutDueToDateTask(IUserService userService, IToDoTaskService toDoTaskService)
        {
            _userService = userService;
            _todoTaskService = toDoTaskService;
        }

        public async Task Run()
        {
            var tasks = await _todoTaskService.GetAll();
            var neededTasks = await tasks.Where(t => t.DueToDate.Value.DayOfYear == DateTime.Now.DayOfYear).ToListAsync();
            foreach ( var task in neededTasks )
            {
                var user = await _userService.GetUserById(task.UserId);
            }
        }
    }
}
