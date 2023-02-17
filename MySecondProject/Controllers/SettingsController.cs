using List_Domain.ViewModel;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MySecondProject.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<ActionResult<ViewSettings>> Get()
        {
            return Ok(await _settingsService.GetSettingsByUser());
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(ViewSettings settings)
        {
            return Ok(await _settingsService.CreateSettings(settings));
        }

        [HttpDelete]
        public async Task<ActionResult<int>> Delete(int id)
        {
            return Ok(await _settingsService.RemoveSettings(id));
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(ViewSettings settings)
        {
            return Ok(await _settingsService.UpdateSetings(settings));
        }
    }
}
