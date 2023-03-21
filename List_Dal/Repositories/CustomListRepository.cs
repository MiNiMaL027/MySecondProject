using List_Dal.Interfaces;
using List_Domain.Exeptions;
using List_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace List_Dal.Repositories
{
    public class CustomListRepository : ICustomListRepository, IChekAuthorization<CustomList>
    {
        private readonly ApplicationContext db;
        private readonly DbSet<CustomList> dbSet;

        public CustomListRepository(ApplicationContext context)
        {
            db = context;
            dbSet = db.Set<CustomList>();
        }

        public async Task<int> Add(CustomList item)
        {
            dbSet.Add(item);
            await db.SaveChangesAsync();

            return item.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True or False</returns>
        public async Task<bool> CheckIfNameExist(string name, int userId)
        {
            return await dbSet.AnyAsync(x => x.Name == name && x.UserId == userId);
        }

        public async Task<IQueryable<CustomList>> GetAll()
        {
            return dbSet;
        }

        public async Task<CustomList> GetById(int Id)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IQueryable<CustomList?>> GetByUser(int userid)
        {
            return dbSet.Where(i => i.ArchivalDate == null && i.UserId == userid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">list id</param>
        /// <returns>True or False</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<int>> Remove(List<int> ids)
        {
            var items = await dbSet.Where(i => ids.Contains(i.Id) && i.ArchivalDate == null).ToListAsync();

            if (items.Count == 0)
                throw new NotFoundException($"{ids} - any from this id not found");

            items.ForEach(x => x.ArchivalDate = DateTime.Now);     
            
            db.SaveChanges();

            return items.Select(i => i.Id).ToList();
        }

        public async Task<bool> RemoveFromDb(List<int> ids)
        {
            var items = await dbSet.Where(i => ids.Contains(i.Id)).ToListAsync();

            if (items.Count == 0)
                return false;

            dbSet.RemoveRange(items);

            db.SaveChanges();

            return true;
        }

        public async Task<bool> Update(CustomList item)
        {
            if (dbSet.Contains(item) && item.ArchivalDate == null)
            {
                dbSet.Update(item);
                await db.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
