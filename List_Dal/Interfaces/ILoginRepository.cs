using List_Domain.Models.NotDbEntity;
using List_Domain.Models;

namespace List_Dal.Interfaces
{
    public interface ILoginRepository
    {
        Task<User> FindLoginModel(string email, string hashed);

        Task<User> GetRegisterModelByEmail(string email);

        Task<EmailConfirmationCode> GetEmailConfirmationCode(string email, string password);

        Task RemoveCode(EmailConfirmationCode code);

        Task AddCode(EmailConfirmationCode code);

        Task<EmailConfirmationCode> GetEmailConfirmationCode(string pass, string email, string confirmationCode);

        Task<List<EmailConfirmationCode>> GetAllEmailConfirmationCodes(string pass, string email);

        Task<int> AddUser(User user);

        Task<bool> Remove(int id);

        Task<bool> RemoveFromDb(List<int> ids);

        Task RemoveFewCode(List<EmailConfirmationCode> Codes);
    }
}
