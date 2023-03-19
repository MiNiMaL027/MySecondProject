using List_Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace List_Service.BackgroundTasks
{
    public class ListRetentionPoleBackgroundTask
    {
        private readonly ICustomListService _listService;

        public ListRetentionPoleBackgroundTask(ICustomListService customListService)
        {
            _listService = customListService;
        }

        public async Task Run()
        {
            var tasks = await _listService.GetAll();
            var ids = await tasks.Where(x => x.ArchivalDate.Value.AddDays(30) >= DateTime.Now).Select(l => l.Id).ToListAsync();

            await _listService.RemoveFromDb(ids);
        }
    }
}
