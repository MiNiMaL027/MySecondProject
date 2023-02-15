namespace List_Dal.Interfaces
{
    public interface IDefaultRepository<T> where T: class?
    {
        Task<int> Add(T item);

        Task<IQueryable<T>> GetByUser(int userId);

        Task<T> GetById(int Id);

        Task<List<int>> Remove(List<int> ids);

        Task<bool> Update(T item);
    }
}
