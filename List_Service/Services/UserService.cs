using List_Domain.Exeptions;
using List_Domain.Models;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace List_Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                throw new NotFoundException();

            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new NotFoundException();

            return user;
        }

        public IQueryable<User> GetAll()
        {
            var users = _userManager.Users;

            if(users == null)
                throw new NotFoundException();

            return users;
        }

        public async Task<bool> RestoreUser(string email)
        {
            var user = await GetByEmail(email);

            user.ArchivalDate = null;

            await _userManager.UpdateAsync(user);

            await _signInManager.SignOutAsync();

            return true;
        }

        public async Task<bool> RemoveUser(int id)
        {
            var user = await GetUserById(id);

            user.ArchivalDate = DateTime.Now;

            await _userManager.UpdateAsync(user);

            await _signInManager.SignOutAsync();

            return true;
        }

        public async Task<bool> RemoveUserFromDb(List<int> ids)
        {
            foreach(var id in ids)
            {
                var user = await GetUserById(id);

                IdentityResult result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                    throw new NotFoundException();
            }
                   
            return true;
        }
    }
}
