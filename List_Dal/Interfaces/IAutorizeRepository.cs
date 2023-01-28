using List_Domain.Models.NotDbEntity;
using List_Domain.Models;

namespace List_Dal.Interfaces
{
    public interface IAutorizeRepository
    {
        Task<User> FindLoginModel(LoginModel model, string hashed);
        Task<User> FindRegisteModel(RegisterModel model);
        Task<EmailConfirmationCode> GetEmailConfirmationCode(RegisterModel model,string hashed);
        Task RemoveCode(EmailConfirmationCode code);
        Task AddCode(EmailConfirmationCode code);
        Task<EmailConfirmationCode> GetEmailConfirmationCode(string pass, string email, string confirmationCode);
        Task<List<EmailConfirmationCode>> GetAllEmailConfirmationCodes(string pass, string email);
        Task<int> AddUser(User user);
        Task RemoveFewCode(List<EmailConfirmationCode> Codes);
    }
}
