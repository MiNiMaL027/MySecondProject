using List_Domain.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MySecondProjectWEB.Views.ViewHelpsModel
{
    public class SettingUserModel : PageModel
    {
        public IQueryable<ViewCustomList> _customList { get; set; }

        public ViewSettings Settings { get; set; }

        public string UserName { get; set; }
    }
}
