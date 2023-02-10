using List_Dal.Interfaces;
using List_Domain.Models.NotDbEntity;
using Microsoft.AspNetCore.Http;

namespace List_Service.Interfaces
{
    public interface IAutorizationService<T> where T : UserEntity
    {
        public void SetUserId(HttpContext httpcContext);

        public int GetUserId();

        public void AuthorizeUser(int id);
    }
}
