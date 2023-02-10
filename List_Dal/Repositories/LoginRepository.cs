using List_Dal.Interfaces;
using List_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace List_Dal.Repositories
{
    public  class LoginRepository : ILoginRepository
    {
        ApplicationContext _db;
        DbSet<User> _dbSetUser;
        DbSet<EmailConfirmationCode> _dbSetComfirmCode;

        public LoginRepository(ApplicationContext context)
        {
            _db = context;
            _dbSetUser = _db.Set<User>();
            _dbSetComfirmCode = _db.Set<EmailConfirmationCode>();
        }

        public async Task AddCode(EmailConfirmationCode code)
        {
            _dbSetComfirmCode.Add(code);
            await _db.SaveChangesAsync();
        }

        public async Task<User?> GetRegisterModelByEmail(string email)
        {
            return await _dbSetUser.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<EmailConfirmationCode?> GetEmailConfirmationCode(string email, string password)
        {
            return await _dbSetComfirmCode.FirstOrDefaultAsync(c => c.Email == email && c.Password == password);
        }

        public async Task<User?> FindLoginModel(string email, string hashed)
        {
            return await _dbSetUser.AsNoTracking().FirstOrDefaultAsync(d => d.Email == email && d.Password == hashed);
        }

        public async Task RemoveCode(EmailConfirmationCode code)
        {
            _dbSetComfirmCode.Remove(code);
            await _db.SaveChangesAsync();
        }

        public async Task<EmailConfirmationCode?> GetEmailConfirmationCode(string pass, string email, string confirmationCode)
        {
            return await _dbSetComfirmCode.FirstOrDefaultAsync(e => e.Email == email && e.Password == pass && e.ConfirmationCode == confirmationCode);
        }

        public async Task<List<EmailConfirmationCode>> GetAllEmailConfirmationCodes(string pass, string email)
        {
            return await _dbSetComfirmCode.Where(e => e.Email == email && e.Password == pass).ToListAsync();
        }

        public async Task<int> AddUser(User user)
        {
            _dbSetUser.Add(user);
            await _db.SaveChangesAsync();

            return user.Id;
        }

        public async Task RemoveFewCode(List<EmailConfirmationCode> сodes)
        {
            _dbSetComfirmCode.RemoveRange(сodes);
            await _db.SaveChangesAsync();
        }     
    }
}
