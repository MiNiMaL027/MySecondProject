using List_Domain.Models;
using List_Domain.Models.NotDbEntity;
using Microsoft.AspNetCore.Mvc;
using List_Service.Interfaces;
using MySecondProject.Filters;

namespace MySecondProject.Controllers
{
    [NotImplExceptionFilter]
    public class AutorizeController : Controller
    {
        private readonly IAutorizeService _service;
        public AutorizeController(IAutorizeService service)
        {
            _service= service;
        }      
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginModel model)
        {
            return Ok(await _service.Login(model));
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterModel model)
        {
            return Ok(await _service.Register(model));
        }

        [HttpGet("sendConfirmationCode/{confirmationCode}")]
        public async Task<ActionResult<UserDTO>> SendConfirmationCode(string confirmationCode)
        {
            string pass = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "TempPass").Value;
            string email = HttpContext.User.Identity.Name;
            return Ok(await _service.SendConfCode(confirmationCode, pass, email));
        }
    }
}
