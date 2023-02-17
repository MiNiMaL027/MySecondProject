using List_Dal.Interfaces;
using List_Domain.Exeptions;
using List_Domain.Models.NotDbEntity;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace List_Service.Services
{
    public class AutorizationService<T> : IAutorizationService<T> where T : UserEntity
    {
        private readonly IChekAuthorization<T> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _currenUserID;

        public AutorizationService(IChekAuthorization<T> repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _currenUserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        }

        public void SetUserId(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new NotFoundException();

            _currenUserID = Convert.ToInt32(httpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        }

        public int GetUserId()
        {
            return _currenUserID;
        }

        public async void AuthorizeUser(int id)
        {
            var item = await _repository.GetById(id);

            if (item == null)
                throw new NotFoundException();

            if(item.UserId != _currenUserID)
                throw new UnautorizeException("No Accessed");
        }
    }
}
