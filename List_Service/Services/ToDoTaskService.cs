using AutoMapper;
using List_Dal.Interfaces;
using List_Domain.CreateModel;
using List_Domain.Enums;
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
        private readonly IAutorizationService<ToDoTask> _authService;

        public ToDoTaskService(IToDoTaskRepository toDoTaskRepository, IMapper mapper, IAutorizationService<ToDoTask> authService)
        {
            _mapper = mapper;
            _todoTaskRepository = toDoTaskRepository;
            _authService = authService;
        }

        public async Task<int> Add(CreateToDoTask item)
        {
            var userId = _authService.GetUserId();

            if (item.Title == null)
                throw new NotFoundException();

            item.Title = item.Title.Trim();

            if (await _todoTaskRepository.CheckIfNameExist(item.Title, userId))
                throw new ValidationException($"{item.Title} - This name is used");

            if (!ValidOptions.ValidOptions.ValidName(item.Title))
                throw new ValidationException($"{item.Title} - Not valide");

            var itemToDb = _mapper.Map<ToDoTask>(item);
            itemToDb.UserId = userId;
            itemToDb.CreationDate = DateTime.Now;

            await _todoTaskRepository.Add(itemToDb);

            return itemToDb.Id;
        }

        public async Task<bool> CompleteTask(int id)
        {
            _authService.AuthorizeUser(id);

            return await _todoTaskRepository.CompleteTask(id);
        }

        public async Task<IQueryable<ViewToDoTask>> GetByListName(string listName)
        {
            if (listName == null)
                throw new NotFoundException();
            listName = listName.Trim();

            var items = await _todoTaskRepository.GetByListName(listName, _authService.GetUserId());

            if (items.Count == 0)
                throw new NotFoundException();

            return items.Select(x => _mapper.Map<ViewToDoTask>(x)).AsQueryable();
        }

        public async Task<IQueryable<ViewToDoTask>> GetByUserId()
        {
            var userId = _authService.GetUserId();
            var items = await _todoTaskRepository.GetByUser(userId);

            return items.ToList().Select(x => _mapper.Map<ViewToDoTask>(x)).AsQueryable();
        }

        public async Task<IQueryable<ViewToDoTask>> GetByBaseList(int baseListId)
        {
            var list = await _todoTaskRepository.GetByUser(_authService.GetUserId());

            switch (baseListId)
            {
                case 1:
                    var dueDateTask = list.Where(x => x.DueToDate != null && x.DueToDate.Value.Date == DateTime.Now.Date);

                    if (dueDateTask.ToList().Count == 0)
                        throw new NotFoundException();

                    return dueDateTask.ToList().Select(x => _mapper.Map<ViewToDoTask>(x)).AsQueryable();
                case 2:
                    var AllTask = list.ToList().Select(x => _mapper.Map<ViewToDoTask>(x)).AsQueryable();

                    if (AllTask.ToList().Count == 0)
                        throw new NotFoundException();

                    return AllTask;
                case 3:
                    var importantTask = list.Where(x => x.Importance > Importance.Low);

                    if (importantTask.ToList().Count == 0)
                        throw new NotFoundException();

                    return importantTask.ToList().Select(x => _mapper.Map<ViewToDoTask>(x)).AsQueryable();
                case 4:
                    var plannedTask = list.Where(x => x.DueToDate != null);

                    if(plannedTask.ToList().Count == 0)
                        throw new NotFoundException();

                    return plannedTask.ToList().Select(x => _mapper.Map<ViewToDoTask>(x)).AsQueryable();
                default: throw new NotFoundException();
            }
        }

        public async Task<List<int>> Remove(List<int> ids)
        {
            foreach (int id in ids)
                _authService.AuthorizeUser(id);

            return await _todoTaskRepository.Remove(ids);
        }

        public async Task<int> Update(CreateToDoTask item, int taskId)
        {
            _authService.AuthorizeUser(taskId);

            if (item.Title == null)
                throw new NotFoundException();

            var userId = _authService.GetUserId();
            item.Title = item.Title.Trim();

            if (await _todoTaskRepository.CheckIfNameExist(item.Title, userId))
                throw new ValidationException($"{item.Title} - This name is used");

            if (!ValidOptions.ValidOptions.ValidName(item.Title))
                throw new ValidationException($"{item.Title} - Not valide");

            var itemToDb = _mapper.Map<ToDoTask>(item);
            itemToDb.Id = taskId;
            itemToDb.UserId = userId;

            if (!await _todoTaskRepository.Update(itemToDb))
                throw new NotFoundException($"{taskId} - Not found");

            return itemToDb.Id;
        }
    }
}
