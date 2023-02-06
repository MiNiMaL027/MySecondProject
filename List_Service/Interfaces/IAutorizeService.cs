using List_Domain.Models.NotDbEntity;
using List_Domain.ModelDTO;

namespace List_Service.Interfaces
{
    public interface IAutorizeService // навіть в інтерфейсах між методами всюди треба пусті лінійки, у всіх файлах
    {
        Task<UserDTO> Login(LoginModel model);
        Task<string> Register(RegisterModel model);
        Task<UserDTO> SendConfCode(string confirmationCode,string pass,string email);
    }
}
