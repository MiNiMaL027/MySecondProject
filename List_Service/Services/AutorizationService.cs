using List_Dal.Interfaces;
using List_Domain.Exeptions;
using List_Domain.Models.NotDbEntity;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace List_Service.Services
{
    public class AutorizationService<T> : IAutorizationService<T> where T : UserEntity
    {
        private readonly IChekAuthorization<T> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AutorizationService(IChekAuthorization<T> repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value == null)
                throw new LoginException();
            return Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public async void AuthorizeUser(int id)
        {
            var item = await _repository.GetById(id);

            if (item == null)
                throw new NotFoundException();

            if(item.UserId != Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value))
                throw new UnautorizeException("No Accessed");
        }
    }
}
