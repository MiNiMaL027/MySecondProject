using List_Dal.Interfaces;
using List_Domain.Models;
using List_Domain.Models.NotDbEntity;
using Microsoft.EntityFrameworkCore;

namespace List_Dal.Repositories
{
    public  class AutorizeRepository : IAutorizeRepository // питань нема, це все зв*язане з авторизацією, АЛЕ одна ететя - один репозиторій, окремо юзерв, окремо конфірмейшинКодів
    {
        ApplicationContext _db;
       /* DbSet<User> _dbSetUser;
        DbSet<EmailConfirmationCode> _dbSetComfirmCode;*/

        public AutorizeRepository(ApplicationContext context)
        {
            _db = context;
            // лишній код
           /* dbSetUser = db.Set<User>();
            dbSetComfirmCode= db.Set<EmailConfirmationCode>();*/
        }

        public async Task AddCode(EmailConfirmationCode code)
        {
            _db.EmailConfirmationCodes.Add(code);
            await _db.SaveChangesAsync();
        }

        public async Task<User?> FindRegisteModel(string email) // мені ця назва нічого не говорить, маєй бути грозуміло, ГетЮзерБайІмейл
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<EmailConfirmationCode?> GetEmailConfirmationCode(string email,string password) // коми
        {
            return _db.EmailConfirmationCodes.FirstOrDefaultAsync(c => c.Email == email && c.Password == password); // де авейт??
        }

        public async Task<User?> FindLoginModel(string email,string hashed) // КОМИ
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(d => d.Email == email && d.Password == hashed);
        }

        public async Task RemoveCode(EmailConfirmationCode code)
        {
            _db.EmailConfirmationCodes.Remove(code);

            await _db.SaveChangesAsync();
        }

        public async Task<EmailConfirmationCode?> GetEmailConfirmationCode(string pass, string email, string confirmationCode)
        {
            return await _db.EmailConfirmationCodes.FirstOrDefaultAsync(e => e.Email == email && e.Password == pass && e.ConfirmationCode == confirmationCode);
        }

        public async Task<List<EmailConfirmationCode>> GetAllEmailConfirmationCodes(string pass, string email)
        {
            return await _db.EmailConfirmationCodes.Where(e => e.Email == email && e.Password == pass).ToListAsync();
        }

        public async Task<int> AddUser(User user)
        {
            _db.Users.Add(user);

            await _db.SaveChangesAsync();

            return user.Id;
        }

        public async Task RemoveFewCode(List<EmailConfirmationCode> Codes) // велика буква в параметрі? серйозно?)
        {
            _db.EmailConfirmationCodes.RemoveRange(Codes);

            await _db.SaveChangesAsync();
        }     
    }
}
