namespace List_Dal.Interfaces
{
    public interface IDefaultRepository<T> where T: class?
    {
        Task<int> Add(T item);

        Task<IQueryable<T>> GetByUser(int userId);

        Task<IQueryable<T>> GetAll();

        Task<List<int>> Remove(List<int> ids);

        Task<bool> Update(T item);

        Task<bool> RemoveFromDb(List<int> ids);
    }
}
