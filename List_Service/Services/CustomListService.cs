using AutoMapper;
using List_Dal.Interfaces;
using List_Domain.CreateModel;
using List_Domain.Exeptions;
using List_Domain.Models;
using List_Domain.ViewModel;
using List_Service.Interfaces;

namespace List_Service.Services
{
    public class CustomListService : ICustomListService
    {
        private readonly ICustomListRepository _customListRepository;
        private readonly IMapper _mapper;
        public CustomListService(ICustomListRepository customListRepository,IMapper mapper)
        {
            _mapper= mapper;
            _customListRepository = customListRepository;
        }

        public async Task<int> Add(CreateCustomList item, int userId)
        {
            item.Name.Trim(); // Боже)) ти хо колись перевіряєш код? воно не працює, і в одному випадеку кидає ексепшн, ТЕСТУВАННЯ ТРЕБА ДЛЯ КОЖНОГО РЯДКА, написав строчку - перевірив чи воно працює, інакше це трата часу девелоперів які це будуть перевіряти, Тестерів які це будуть тестити і тебе, який якому код будуть завертати по 100 разів через такі "ай махав я тестити"

            if (await _customListRepository.CheckIfNameExist(item.Name,userId))
                throw new ValidProblemException($"{item.Name} - This name is used"); // погана назва, Валідна проблема експешн)) ВалідейшнЕксепшн

            if (!ValidOptions.ValidOptions.ValidName(item.Name))
                throw new ValidProblemException($"{item.Name} - Not valide");

            var itemToDb = _mapper.Map<CustomList>(item);
            itemToDb.UserId= userId; // пробіл
            
            await _customListRepository.Add(itemToDb);

            return itemToDb.Id;
        }

        public async Task<IQueryable<CustomListView>> Get(int userid) // гет бай бзер айді, а ще треба метод просто гет бай айді, одне
        {
            var items = await _customListRepository.Get(userid);

            return items.ToList().Select(x => _mapper.Map<CustomListView>(x)).AsQueryable();
        }

        public async Task<List<int>> Remove(List<int> ids, int userId)
        {
            return await _customListRepository.Remove(ids,userId);// коми
        }

        public async Task<int> Update(CreateCustomList item, int userId, int listId)
        {
            item.Name.Trim();// ААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААААА

            if (await _customListRepository.CheckIfNameExist(item.Name, userId))
                throw new ValidProblemException($"{item.Name} - This name is used");

            if (!ValidOptions.ValidOptions.ValidName(item.Name))
                throw new ValidProblemException($"{item.Name} - Not valide");

            var itemToDb = _mapper.Map<CustomList>(item);
            itemToDb.Id = listId;
            itemToDb.UserId = userId; // сервіс має сам витягувати юезр айді, а не бути переданим від контроллера, я попереджав, виправляй

            if (!await _customListRepository.Update(itemToDb, userId))
                throw new NotFoundException($"{listId} - can't be Foud");

            return itemToDb.Id;
        }
    }
}
