using AutoMapper;
using AutoMapper.QueryableExtensions;
using List_Dal.Interfaces;
using List_Domain.CreateModel;
using List_Domain.Exeptions;
using List_Domain.Models;
using List_Domain.ViewModel;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace List_Service.Services
{
    public class CustomListService : ICustomListService
    {
        private readonly ICustomListRepository _customListRepository;
        private readonly IMapper _mapper;
        private readonly IAutorizationService<CustomList> _authService;

        public CustomListService(ICustomListRepository customListRepository, IMapper mapper, IAutorizationService<CustomList> authService)
        {
            _mapper= mapper;
            _customListRepository = customListRepository;
            _authService = authService;
        }

        public DefaultHttpContext HttpContext { get; set; }

        public async Task<int> Add(CreateCustomList item)
        {
            var userId = _authService.GetUserId();

            ValidOptions.ValidOptions.ValidNameCreateModel(item.Name);

            if (await _customListRepository.CheckIfNameExist(item.Name, userId))
                throw new ValidationException($"{item.Name} - This name is used");

            var itemToDb = _mapper.Map<CustomList>(item);
            itemToDb.UserId = userId;
            
            await _customListRepository.Add(itemToDb);

            return itemToDb.Id;
        }

        public async Task<IQueryable<ViewCustomList>> GetAll()
        {
            var items = await _customListRepository.GetAll();

            return items.ProjectTo<ViewCustomList>(_mapper.ConfigurationProvider);
        }

        public async Task<IQueryable<ViewCustomList>> GetByUserId()
        {
            var userId = _authService.GetUserId();
            var items = await _customListRepository.GetByUser(userId);

            if (items == null)
                throw new NotFoundException();
      
            return items.ProjectTo<ViewCustomList>(_mapper.ConfigurationProvider);

            //return items.ToList().Select(x => _mapper.Map<ViewCustomList>(x)).AsQueryable();
        }

        public async Task<List<int>> Remove(List<int> ids)
        {
            foreach (int id in ids)
                _authService.AuthorizeUser(id);

            return await _customListRepository.Remove(ids);
        }

        public async Task<bool> RemoveFromDb(List<int> ids)
        {
            return await _customListRepository.RemoveFromDb(ids);
        }

        public async Task<int> Update(CreateCustomList item, int listId)
        {
            _authService.AuthorizeUser(listId);

            var userId = _authService.GetUserId();

            ValidOptions.ValidOptions.ValidNameCreateModel(item.Name);

            if (await _customListRepository.CheckIfNameExist(item.Name, userId))
                throw new ValidationException($"{item.Name} - This name is used");   

            var itemToDb = _mapper.Map<CustomList>(item);
            itemToDb.Id = listId;
            itemToDb.UserId = userId;

            if (!await _customListRepository.Update(itemToDb))
                throw new NotFoundException($"{listId} - can't be Foud");

            return itemToDb.Id;
        }
    }
}
