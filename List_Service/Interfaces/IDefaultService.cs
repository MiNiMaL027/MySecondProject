
namespace List_Service.Interfaces
{
    public interface IDefaultService<T,C> where T : class? where C : class? // замість Т С назив краще РеквестТайп, РеспонсуТайп, поки не зрозуміло що є що
    {
        Task<int> Add(C item,int userid);
        Task<IQueryable<T>> Get(int userId);
        Task<List<int>> Remove(List<int> ids, int uresId);
        Task<int> Update(C item,int userId, int itemId);
    }
}
