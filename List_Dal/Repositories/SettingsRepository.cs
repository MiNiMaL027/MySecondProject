using List_Dal.Interfaces;
using List_Domain.Models;
using Microsoft.EntityFrameworkCore;
using List_Domain.Exeptions;
using System.Collections.Generic;

namespace List_Dal.Repositories
{
    public class SettingsRepository : ISettingsRepository, IChekAuthorization<Settings>
    {
        private readonly ApplicationContext _db;
        private readonly DbSet<Settings> _dbSet;

        public SettingsRepository(ApplicationContext context)
        {
            _db = context;
            _dbSet = _db.Set<Settings>();
        }

        public async Task<int> CreateSettings(Settings settings)
        {
            _dbSet.Add(settings);
            await _db.SaveChangesAsync();

            return settings.Id;
        }

        public async Task<Settings> GetById(int Id)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<Settings> GetSettingsByUser(int userId)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<int> RemoveSettings(int id)
        {
            var item = await _dbSet.FirstOrDefaultAsync(s => s.Id == id);

            if (item == null)
                throw new NotFoundException();

            _dbSet.Remove(item);
            _db.SaveChanges();

            return id;
        }

        public async Task<bool> UpdateSetings(Settings settings)
        {
            if (_dbSet.Contains(settings))
            {
                _dbSet.Update(settings);
                await _db.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
