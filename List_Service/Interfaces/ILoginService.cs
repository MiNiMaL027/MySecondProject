using List_Domain.Models.NotDbEntity;
using List_Domain.ModelDTO;
using Microsoft.AspNetCore.Http;

namespace List_Service.Interfaces
{
    public interface ILoginService 
    {
        Task<UserDTO> Login(LoginModel model);

        Task<string> Register(RegisterModel model);

        Task<UserDTO> SendConfCode(string confirmationCode);

        void SetHttpContext(HttpContext httpContext);
    }
}
