using List_Domain.Models.NotDbEntity;
using Microsoft.AspNetCore.Mvc;
using List_Service.Interfaces;
using List_Service.Filters;
using List_Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace MySecondProject.Controllers
{
    [AllowAnonymous]
    [NotImplExceptionFilter]
    public class LoginController : Controller
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service= service;
        }

        [Authorize]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginModel model)
        {
            return Ok(await _service.SignIn(model));
        }

        [HttpDelete("logOut")]
        public async Task<ActionResult> LogOut()
        {
            _service.SignOff();
            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterModel model)
        {
            return Ok(await _service.Register(model));
        }
    }
}
