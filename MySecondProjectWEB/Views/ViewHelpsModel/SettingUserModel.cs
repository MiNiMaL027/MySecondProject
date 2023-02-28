using List_Domain.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MySecondProjectWEB.Views.ViewHelpsModel
{
    public class SettingUserModel : PageModel
    {
        public IQueryable<ViewCustomList> _customList { get; set; } 
        // шо за прикол? де ти бачив щоб так проперті називали, ні,
        // всюди і завжди проперті одинаково З великої букви, прогуглити про КамелКейс і Паскал кейс шоб розуміти коли тобі таке слово скажуть

        public ViewSettings Settings { get; set; }

        public string UserName { get; set; }
    }
}
