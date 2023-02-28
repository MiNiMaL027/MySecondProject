using List_Domain.Exeptions;
using List_Domain.Models;
using List_Domain.Models.NotDbEntity;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace List_Service.Services
{
    [AllowAnonymous]
    public class LoginService : ILoginService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public async Task<bool> SignIn(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (user == null)
                throw new LoginException();

            if (result == null)
                throw new LoginException();

            if (result.Succeeded)
                return true;

            if (result.RequiresTwoFactor)
                throw new LoginException();

            if (result.IsLockedOut)
                throw new LoginException();

            throw new LoginException();
        }

        public async void SignOff()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<User> Register(RegisterModel model)
        {
            if (model.Password != model.ConfirmedPassword)
                throw new LoginException();

            var user = new User
            {
                Name = model.Name,
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return user;
            }

            throw new LoginException();
        }
    }
}
