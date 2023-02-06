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
        public async Task<bool> CheckIfNameExist(string name, int userId) // ого, автодокументація, а чо пуста?
        {
            return await dbSet.AnyAsync(x => x.Name == name && x.UserId == userId);
        }

        public async Task<IQueryable<CustomList?>> Get(int userid) // гетОЛЛ
        {
            return dbSet.Where(i => !i.IsDeleted && i.UserId == userid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">list id</param>
        /// <returns>True or False</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<int>> Remove(List<int> ids, int userId)
        {
            var items = await dbSet.Where(i => ids.Contains(i.Id) && i.UserId == userId && !i.IsDeleted).ToListAsync(); // Для чого тут ЮзерАйді?

            if (items == null)
                throw new NotFoundException($"{ids} - any from this id not found");

            // по-перше ніхто не використовує фор коли можна форіч, по-друге нашо писати то в три лінійки з лишніми зміннами коли можна в рядок??
            /*for (int i = 0; i < items.Count; i++) 
            {           
                items[i].IsDeleted = true;
            }*/

            items.ForEach(x => x.IsDeleted = true);

            db.SaveChanges();

            return items.Select(i => i.Id).ToList(); //ToList тут точно потрібне?
        }

        public async Task<bool> Update(CustomList item, int userid) // для чого юзер айді? айдішки листа не пересікаються
        {
            if (item == null)
                throw new NotFoundException($"{item.Id} - Not found");// ахахха, ну читай ти трохи що пише під хвилястою лінією)) якщо в тебе айтем нал, то якшо ти зробиш item.Id воно кине тобі НуллРеференс ексепшн, ти змусив мене сміятися, випадково написати таку смішну багу важно😂😅

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
