using List_Domain.Exeptions;
using List_Domain.Models.NotDbEntity;
using List_Domain.ViewModel;
using List_Service.Interfaces;
using List_Service.Services;
using Microsoft.AspNetCore.Mvc;
using MySecondProjectWEB.Views.ViewHelpsModel;
using System.Security.Claims;

namespace MySecondProjectWEB.Controllers
{
    public class ProfilController : Controller
    {
        private readonly ISettingsService _settingsService;
        private readonly ICustomListService _customListService;
        private readonly IUserService _userService;

        public ProfilController(ISettingsService settingsService, ICustomListService customListService, IUserService userService)
        {
            _customListService = customListService;
            _settingsService = settingsService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            try
            {
                var model = new SettingUserModel()
                {
                    UserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value ?? "Name not found",
                    Settings = await _settingsService.GetSettingsByUser(),
                    CustomList = await _customListService.GetByUserId()
                };

                return View(model);
            }
            catch (NotFoundException)
            {
                return View(new SettingUserModel() 
                {
                    UserName = HttpContext.User.Identity.Name,
                    CustomList = await _customListService.GetByUserId() }
                ); 
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

        [HttpPost]
        public async Task<IActionResult> DeleteProfil()
        {
            await _userService.RemoveUser(Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public async Task<IActionResult> RestoreProfil()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RestoreProfil(string email)
        {
            await _userService.RestoreUser(email);

            return RedirectToAction("Login", "Login");
        }
    }
}
