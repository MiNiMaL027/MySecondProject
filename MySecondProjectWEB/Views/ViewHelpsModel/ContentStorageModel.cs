using List_Domain.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MySecondProjectWEB.Views.ViewHelpsModel // ц ц ц, вчимо англійську краще) просто вюмоделс
{
    public class ContentStorageModel : PageModel
    {
        public IQueryable<ViewCustomList> _customLists; //АХХААХ, виправити

        public IQueryable<ViewToDoTask> _toDoTasks;
    }
}
