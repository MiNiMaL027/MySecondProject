namespace List_Dal.Interfaces
{
    public interface IDefaultRepository<T> where T: class?
    {
        Task<int> Add(T item);
        Task<IQueryable<T>> Get(int userId);
        Task<List<int>> Remove(List<int> ids,int userId);
        Task<bool> Update(T item,int userId);
    }
}
