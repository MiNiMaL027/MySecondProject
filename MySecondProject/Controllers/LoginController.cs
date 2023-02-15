using List_Domain.Models.NotDbEntity;
using Microsoft.AspNetCore.Mvc;
using List_Service.Interfaces;
using MySecondProject.Filters;
using List_Domain.ModelDTO;

namespace MySecondProject.Controllers
{
    [NotImplExceptionFilter]
    public class LoginController : Controller
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service)
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
            _service.SetHttpContext(HttpContext);
            return Ok(await _service.SendConfCode(confirmationCode));
        }
    }
}
