using AutoMapper;
using List_Dal.Interfaces;
using List_Domain.CreateModel;
using List_Domain.Exeptions;
using List_Domain.Models;
using List_Domain.ViewModel;
using List_Service.Interfaces;

namespace List_Service.Services
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly IToDoTaskRepository _todoTaskRepository;
        private readonly IMapper _mapper;

        public ToDoTaskService(IToDoTaskRepository toDoTaskRepository, IMapper mapper)
        {
            _mapper = mapper;
            _todoTaskRepository = toDoTaskRepository;
        }

        public async Task<int> Add(CreateToDoTask item, int userId)
        {
            item.Title.Trim();

            if (await _todoTaskRepository.CheckIfNameExist(item.Title, userId))
                throw new ValidProblemException($"{item.Title} - This name is used");

            if (!ValidOptions.ValidOptions.ValidName(item.Title))
                throw new ValidProblemException($"{item.Title} - Not valide");

            var itemToDb = _mapper.Map<ToDoTask>(item);
            itemToDb.UserId = userId;
            itemToDb.CreationDate = DateTime.Now;

            await _todoTaskRepository.Add(itemToDb);

            return itemToDb.Id;
        }

        public async Task<bool> CompleteTask(int id, int userId)
        {
            return await _todoTaskRepository.CompleteTask(id, userId);
        }

        public async Task<IQueryable<ToDoTaskView>> Get(int userId)
        {
            var items = await _todoTaskRepository.Get(userId);

            return items.ToList().Select(x => _mapper.Map<ToDoTaskView>(x)).AsQueryable();
        }

        public async Task<List<int>> Remove(List<int> ids, int uresId)
        {
            return await _todoTaskRepository.Remove(ids, uresId);
        }

        public async Task<int> Update(CreateToDoTask item, int userId, int taskId)
        {          
            item.Title.Trim();

            if (await _todoTaskRepository.CheckIfNameExist(item.Title, userId))
                throw new ValidProblemException($"{item.Title} - This name is used");

            if (!ValidOptions.ValidOptions.ValidName(item.Title))
                throw new ValidProblemException($"{item.Title} - Not valide");

            var itemToDb = _mapper.Map<ToDoTask>(item);
            itemToDb.Id = taskId;
            itemToDb.UserId = userId;

            if (!await _todoTaskRepository.Update(itemToDb, userId))
                throw new NotFoundException($"{taskId} - Not found");

            return itemToDb.Id;
        }
    }
}
