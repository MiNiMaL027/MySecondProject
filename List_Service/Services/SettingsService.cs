using AutoMapper;
using List_Dal.Interfaces;
using List_Domain.Exeptions;
using List_Domain.Models;
using List_Domain.ViewModel;
using List_Service.Interfaces;

namespace List_Service.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _repository;
        private readonly IAutorizationService<Settings> _authService;
        private readonly IMapper _mapper;

        public SettingsService(ISettingsRepository repository, IAutorizationService<Settings> auhtService, IMapper mapper)
        {
            _repository = repository;
            _authService = auhtService;
            _mapper = mapper;
        }

        public async Task<int> CreateSettings(ViewSettings settings)
        {
            var item = await _repository.GetSettingsByUser(_authService.GetUserId());

            if (item != null)
                throw new ValidationException();

             var itemToDb = _mapper.Map<Settings>(settings);
            itemToDb.UserId = _authService.GetUserId();

            await _repository.CreateSettings(itemToDb);
           
            return itemToDb.Id;
        }

        public async Task<ViewSettings> GetSettingsByUser()
        {
            var userId = _authService.GetUserId();
            var item = await _repository.GetSettingsByUser(userId);

            if(item == null)
                throw new NotFoundException();

            return _mapper.Map<ViewSettings>(item);
        }

        public async Task<int> RemoveSettings(int id)
        {
            _authService.AuthorizeUser(id);

            return await _repository.RemoveSettings(id);
        }
     
        public async Task<int> UpdateSetings(ViewSettings settings)
        {                                                                   // Тут можна б було не тягнути айтем з БД,а зробити шо настройки мають таке саме айді як і Юзер,але ти скоріше за все скажеш шо то фігня)
            var userId = _authService.GetUserId();
            var item = await _repository.GetSettingsByUser(userId);

            _authService.AuthorizeUser(item.Id);

            var itemToDb = _mapper.Map<Settings>(settings);

            itemToDb.UserId = userId;
            itemToDb.Id = item.Id;

            if(!await _repository.UpdateSetings(itemToDb))
                throw new NotFoundException();

            return itemToDb.Id;
        }
    }
}
