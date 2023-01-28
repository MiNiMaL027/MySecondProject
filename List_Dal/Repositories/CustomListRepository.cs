using List_Dal.Interfaces;
using List_Domain.Exeptions;
using List_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace List_Dal.Repositories
{
    public class CustomListRepository : ICustomListRepository
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
        public async Task<bool> CheckIfNameExist(string name,int userId)
        {
            return await dbSet.AnyAsync(x => x.Name == name && x.UserId == userId);
        }

        public async Task<IQueryable<CustomList?>> Get(int userid)
        {
            return dbSet.Where(i => !i.IsDeleted && i.UserId == userid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">list id</param>
        /// <returns>True or False</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<int>> Remove(List<int> ids,int userId)
        {
            var items = await dbSet.Where(i => ids.Contains(i.Id) && i.UserId == userId && !i.IsDeleted).ToListAsync();
            if (items == null)
                throw new NotFoundException($"{ids} - any from this id not found");
            for (int i = 0; i < items.Count; i++)
            {           
                items[i].IsDeleted = true;
            }           
            db.SaveChanges();
            return items.Select(i => i.Id).ToList();
        }

        public async Task<bool> Update(CustomList item,int userid)
        {
            if (item == null)
                throw new NotFoundException($"{item.Id} - Not found");
            if (dbSet.Contains(item) && !item.IsDeleted && item.UserId == userid)
            {
                dbSet.Update(item);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
