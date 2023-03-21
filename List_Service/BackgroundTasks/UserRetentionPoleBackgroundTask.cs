using List_Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace List_Service.BackgroundTasks
{
    public class UserRetentionPoleBackgroundTask
    {
        private readonly IUserService _userService;

        public UserRetentionPoleBackgroundTask(IUserService userService)
        {
            _userService = userService;
        }

        public async Task Run()
        {
            var ids = await _userService.GetAll().Where(u => u.ArchivalDate.Value.AddDays(30) <= DateTime.Now).Select(x => x.Id).ToListAsync();

            await _userService.RemoveUserFromDb(ids);
        }
    }
}
