using List_Dal.Interfaces;
using List_Domain.Exeptions;
using List_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace List_Dal.Repositories
{
    public class ToDoTaskRepository : IToDoTaskRepository
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

        public async Task<bool> CompleteTask(int id, int userId)
        {
            var task = await dbSet.FirstOrDefaultAsync(i => !i.IsDeleted && i.UserId == userId && i.Id == id);

            if (task == null)
                throw new NotFoundException($"{id} - Not Found");

            if (!task.IsCompleted)
            {
                task.IsDeleted= true;
                task.IsCompleted = true;

                return true;
            }

            return false;
                
        }

        public async Task<IQueryable<ToDoTask>> Get(int userId)
        {
            return dbSet.Where(i => i.UserId == userId && !i.IsDeleted).AsQueryable();
        }

        public async Task<List<int>> Remove(List<int> ids, int userId)
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

        public async Task<bool> Update(ToDoTask item, int userId)
        {
            if (item == null)
                throw new NullReferenceException();

            if (dbSet.Contains(item) && !item.IsDeleted && !item.IsCompleted && item.UserId == userId)
            {
                dbSet.Update(item);
                await db.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
