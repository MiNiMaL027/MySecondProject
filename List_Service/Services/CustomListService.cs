using AutoMapper;
using List_Dal.Interfaces;
using List_Domain.CreateModel;
using List_Domain.Exeptions;
using List_Domain.Models;
using List_Domain.ViewModel;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace List_Service.Services
{
    public class CustomListService : ICustomListService
    {
        private readonly ICustomListRepository _customListRepository;
        private readonly IMapper _mapper;
        private readonly IAutorizationService<CustomList> _authService;

        private HttpContext? _httpContext;

        public CustomListService(ICustomListRepository customListRepository, IMapper mapper, IAutorizationService<CustomList> authService)
        {
            _mapper= mapper;
            _customListRepository = customListRepository;
            _authService = authService;
        }

        public void SetHttpContext(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public async Task<int> Add(CreateCustomList item)
        {
            _authService.SetUserId(_httpContext);

            var userId = _authService.GetUserId();

            item.Name = item.Name.Trim();

            if (await _customListRepository.CheckIfNameExist(item.Name,userId))
                throw new ValidationException($"{item.Name} - This name is used");

            if (!ValidOptions.ValidOptions.ValidName(item.Name))
                throw new ValidationException($"{item.Name} - Not valide");

            var itemToDb = _mapper.Map<CustomList>(item);
            itemToDb.UserId = userId;
            
            await _customListRepository.Add(itemToDb);

            return itemToDb.Id;
        }

        public async Task<IQueryable<ViewCustomList>> GetByUserId()
        {
            _authService.SetUserId(_httpContext);

            var userId = _authService.GetUserId();
            var items = await _customListRepository.GetByUser(userId);

            return items.ToList().Select(x => _mapper.Map<ViewCustomList>(x)).AsQueryable();
        }

        public async Task<List<int>> Remove(List<int> ids)
        {
            _authService.SetUserId(_httpContext);

            foreach (int id in ids)
                _authService.AuthorizeUser(id);

            return await _customListRepository.Remove(ids);
        }

        public async Task<int> Update(CreateCustomList item, int listId)
        {
            _authService.SetUserId(_httpContext);
            _authService.AuthorizeUser(listId);

            var userId = _authService.GetUserId();
            item.Name = item.Name.Trim();

            if (await _customListRepository.CheckIfNameExist(item.Name, userId))
                throw new ValidationException($"{item.Name} - This name is used");

            if (!ValidOptions.ValidOptions.ValidName(item.Name))
                throw new ValidationException($"{item.Name} - Not valide");

            var itemToDb = _mapper.Map<CustomList>(item);
            itemToDb.Id = listId;
            itemToDb.UserId = userId;

            if (!await _customListRepository.Update(itemToDb))
                throw new NotFoundException($"{listId} - can't be Foud");

            return itemToDb.Id;
        }
    }
}
