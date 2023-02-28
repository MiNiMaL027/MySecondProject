using List_Dal.Interfaces;
using List_Domain.Exeptions;
using List_Domain.Models;
using Microsoft.EntityFrameworkCore;

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
            var task = await dbSet.FirstOrDefaultAsync(i => !i.IsDeleted && i.Id == id);
            if (task == null)
                throw new NotFoundException($"{id} - Not Found");

            if (!task.IsCompleted)
            {
                task.IsDeleted = true;
                task.IsCompleted = true;
                return true;
            }

            return false;            
        }

        public async Task<ToDoTask> GetById(int Id)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<List<ToDoTask>> GetByListName(string listName, int userId)
        {
            var list = await db.Set<CustomList>().FirstAsync(x => x.Name == listName && x.UserId == userId && x.IsDeleted == false);

            if (list == null)
                throw new NotFoundException($"{listName} -- Not Exist");

            return await dbSet.Where(t => t.CustomListId == list.Id && t.IsDeleted == false && t.UserId == userId).ToListAsync();
        }

        public async Task<IQueryable<ToDoTask>> GetByUser(int userId)
        {
            return dbSet.Where(i => i.UserId == userId && !i.IsDeleted);
        }

        public async Task<List<int>> Remove(List<int> ids)
        {
            var items = await dbSet.Where(i => ids.Contains(i.Id) && !i.IsDeleted).ToListAsync();

            if (items.Count == 0)
                throw new NotFoundException($"{ids} - any from this id not found");

            items.ForEach(x => x.IsDeleted = true);

            db.SaveChanges();

            return items.Select(i => i.Id).ToList();
        }

        public async Task<bool> Update(ToDoTask item)
        {
            if (item == null)
                throw new NullReferenceException();

            if (dbSet.Contains(item) && !item.IsDeleted && !item.IsCompleted)
            {
                dbSet.Update(item);
                await db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
