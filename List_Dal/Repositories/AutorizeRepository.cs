using List_Dal.Interfaces;
using List_Domain.Models;
using List_Domain.Models.NotDbEntity;
using Microsoft.EntityFrameworkCore;

namespace List_Dal.Repositories
{
    public  class AutorizeRepository : IAutorizeRepository
    {
        ApplicationContext db;
        DbSet<User> dbSetUser;
        DbSet<EmailConfirmationCode> dbSetComfirmCode;

        public AutorizeRepository(ApplicationContext context)
        {
            db = context;
            dbSetUser = db.Set<User>();
            dbSetComfirmCode= db.Set<EmailConfirmationCode>();
        }

        public async Task AddCode(EmailConfirmationCode code)
        {
            dbSetComfirmCode.Add(code);
            await db.SaveChangesAsync();
        }

        public async Task<User?> FindRegisteModel(RegisterModel model)
        {
            return await dbSetUser.AsNoTracking().FirstOrDefaultAsync(u => u.Email == model.Email);
        }

        public Task<EmailConfirmationCode?> GetEmailConfirmationCode(RegisterModel model, string hashed)
        {
            return dbSetComfirmCode.FirstOrDefaultAsync(c => c.Email == model.Email && c.Password == model.Password);
        }


        public async Task<User?> FindLoginModel(LoginModel model,string hashed)
        {
            return await dbSetUser.AsNoTracking().FirstOrDefaultAsync(d => d.Email == model.Email && d.Password == hashed);
        }

        public async Task RemoveCode(EmailConfirmationCode code)
        {
            dbSetComfirmCode.Remove(code);
            await db.SaveChangesAsync();
        }

        public async Task<EmailConfirmationCode?> GetEmailConfirmationCode(string pass, string email, string confirmationCode)
        {
            return await dbSetComfirmCode.FirstOrDefaultAsync(e => e.Email == email && e.Password == pass && e.ConfirmationCode == confirmationCode);
        }

        public async Task<List<EmailConfirmationCode>> GetAllEmailConfirmationCodes(string pass, string email)
        {
            return await dbSetComfirmCode.Where(e => e.Email == email && e.Password == pass).ToListAsync();
        }

        public async Task<int> AddUser(User user)
        {
            dbSetUser.Add(user);
            await db.SaveChangesAsync();
            return user.Id;
        }

        public async Task RemoveFewCode(List<EmailConfirmationCode> Codes)
        {
            dbSetComfirmCode.RemoveRange(Codes);
            await db.SaveChangesAsync();
        }     
    }
}
