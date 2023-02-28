using List_Domain.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MySecondProjectWEB.Views.ViewHelpsModel
{
    public class ContentStorageModel : PageModel
    {
        public IQueryable<ViewCustomList> _customLists;

        public IQueryable<ViewToDoTask> _toDoTasks;
    }
}
