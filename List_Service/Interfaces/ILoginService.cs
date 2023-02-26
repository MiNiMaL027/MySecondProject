using List_Domain.Models;
using List_Domain.Models.NotDbEntity;

namespace List_Service.Interfaces
{
    public interface ILoginService 
    {
        Task<bool> SignIn(LoginModel model);

        void SignOff();

        Task<User> Register(RegisterModel model);
    }
}
