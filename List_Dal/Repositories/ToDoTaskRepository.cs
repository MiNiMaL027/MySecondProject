using List_Dal.Interfaces;
using List_Domain.Exeptions;
using List_Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace List_Dal.Repositories
{
    public class ToDoTaskRepository : IToDoTaskRepository ,IChekAuthorization<ToDoTask>
    {
        private readonly ApplicationContext db;
        private readonly DbSet<ToDoTask> dbSet;

        public ToDoTaskRepository(ApplicationContext context)
        {
            db = context;
            dbSet = db.Set<ToDoTask>();
        }

        public async Task<int> Add(ToDoTask item)
        {
            dbSet.Add(item);
            await db.SaveChangesAsync();

            return item.Id;
        }

        public async Task<bool> CheckIfNameExist(string title, int userId)
        {
            return await dbSet.AnyAsync(i => i.Title == title && i.UserId == userId);
        }

        public async Task<bool> CompleteTask(int id)
        {
            var task = await dbSet.FirstOrDefaultAsync(i => i.ArchivalDate == null && i.Id == id);
            if (task == null)
                throw new NotFoundException($"{id} - Not Found");

            if (!task.IsCompleted)
            {
                task.ArchivalDate = DateTime.Now;
                task.IsCompleted = true;
                return true;
            }

            return false;            
        }

        public async Task<IQueryable<ToDoTask>> GetAll()
        {
            return dbSet;
        }

        public async Task<ToDoTask> GetById(int Id)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<List<ToDoTask>> GetByListName(string listName, int userId)
        {
            var list = await db.Set<CustomList>().FirstAsync(x => x.Name == listName && x.UserId == userId && x.ArchivalDate == null);

            if (list == null)
                throw new NotFoundException($"{listName} -- Not Exist");

            return await dbSet.Where(t => t.CustomListId == list.Id && t.ArchivalDate == null && t.UserId == userId).ToListAsync();
        }

        public async Task<IQueryable<ToDoTask>> GetByUser(int userId)
        {
            return dbSet.Where(i => i.UserId == userId && i.ArchivalDate == null);
        }

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

        public async Task<bool> Update(ToDoTask item)
        {
            if (item == null)
                throw new NullReferenceException();

            if (dbSet.Contains(item) && item.ArchivalDate == null && !item.IsCompleted)
            {
                dbSet.Update(item);
                await db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
