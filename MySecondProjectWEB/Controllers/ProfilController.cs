using List_Domain.Exeptions;
using List_Domain.ViewModel;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MySecondProjectWEB.Views.ViewHelpsModel;

namespace MySecondProjectWEB.Controllers
{
    public class ProfilController : Controller
    {
        private readonly ISettingsService _settingsService;

        private readonly ICustomListService _customListService;

        public ProfilController(ISettingsService settingsService, ICustomListService customListService)
        {
            _customListService = customListService;
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            try
            {
                var model = new SettingUserModel() { UserName = HttpContext.User.Identity.Name, Settings = await _settingsService.GetSettingsByUser(), _customList = await _customListService.GetByUserId()};

                return View(model);
            }
            catch (NotFoundException)
            {
                return View(new SettingUserModel() { UserName = HttpContext.User.Identity.Name, _customList = await _customListService.GetByUserId() }); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSettings(ViewSettings settings)
        {
            await _settingsService.CreateSettings(settings);

            return Redirect("Profil");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSettings(ViewSettings settings)
        {
            await _settingsService.UpdateSetings(settings);

            return Redirect("Profil");
        }
    }
}
