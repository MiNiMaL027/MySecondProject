using List_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MySecondProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly ISendEmailService _sendEmailService;

        public UserController(IUserService userService, ISendEmailService sendEmailService)
        {
            _userService = userService;
            _sendEmailService = sendEmailService;
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteUser()
        {
            return await _userService.RemoveUser(Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value));
        }

        [HttpPut]
        public async Task<ActionResult<bool>> RestoreUser(string email)
        {
            return await _userService.RestoreUser(email);
        }

        [HttpPost]
        public async Task<ActionResult> SendMessage(string message)
        {
            await _sendEmailService.SendEmailAsync("Andriy321@meta.ua", "How are you", message);

            return Ok();
        }
    }
}
