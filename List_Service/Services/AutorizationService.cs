using List_Dal.Interfaces;
using List_Domain.Exeptions;
using List_Domain.Models.NotDbEntity;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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
            _currenUserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public void SetUserId(int id)
        {
            _currenUserID = id;
        }

        public int GetUserId()
        {
            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value == null)
                throw new LoginException();
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
