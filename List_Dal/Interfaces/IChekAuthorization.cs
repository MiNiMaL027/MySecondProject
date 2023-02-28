namespace List_Dal.Interfaces
{
    public interface IChekAuthorization<T>
    {
        Task<T> GetById(int Id);
    }
}
