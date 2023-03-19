using AutoMapper;
using AutoMapper.QueryableExtensions;
using List_Dal.Interfaces;
using List_Dal.Repositories;
using List_Domain.CreateModel;
using List_Domain.Enums;
using List_Domain.Exeptions;
using List_Domain.Models;
using List_Domain.ViewModel;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace List_Service.Services
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly IToDoTaskRepository _todoTaskRepository;
        private readonly IMapper _mapper;
        private readonly IAutorizationService<ToDoTask> _authService;

        public DefaultHttpContext HttpContext { get; set; }

        public ToDoTaskService(IToDoTaskRepository toDoTaskRepository, IMapper mapper, IAutorizationService<ToDoTask> authService)
        {
            _mapper = mapper;
            _todoTaskRepository = toDoTaskRepository;
            _authService = authService;
        }

        public async Task<int> Add(CreateToDoTask item)
        {
            var userId = _authService.GetUserId();

            ValidOptions.ValidOptions.ValidNameCreateModel(item.Title);
            if (!ValidOptions.ValidOptions.ValidDueDate(item.DueToDate))
                item.DueToDate = null;

            if (await _todoTaskRepository.CheckIfNameExist(item.Title, userId))
                throw new ValidationException($"{item.Title} - This name is used");

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

            return items.ProjectTo<ViewToDoTask>(_mapper.ConfigurationProvider);
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

                    return dueDateTask.ProjectTo<ViewToDoTask>(_mapper.ConfigurationProvider);

                case 2:
                    var AllTask = list.ProjectTo<ViewToDoTask>(_mapper.ConfigurationProvider);

                    if (AllTask.ToList().Count == 0)
                        throw new NotFoundException();

                    return AllTask;

                case 3:
                    var importantTask = list.Where(x => x.Importance > Importance.Low);

                    if (importantTask.ToList().Count == 0)
                        throw new NotFoundException();

                    return importantTask.ProjectTo<ViewToDoTask>(_mapper.ConfigurationProvider);

                case 4:
                    var plannedTask = list.Where(x => x.DueToDate != null);

                    if(plannedTask.ToList().Count == 0)
                        throw new NotFoundException();

                    return plannedTask.ProjectTo<ViewToDoTask>(_mapper.ConfigurationProvider);

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

            ValidOptions.ValidOptions.ValidNameCreateModel(item.Title);
            if (!ValidOptions.ValidOptions.ValidDueDate(item.DueToDate))
                item.DueToDate = null;

            var userId = _authService.GetUserId();

            if (await _todoTaskRepository.CheckIfNameExist(item.Title, userId))
                throw new ValidationException($"{item.Title} - This name is used");

            var itemToDb = _mapper.Map<ToDoTask>(item);
            itemToDb.Id = taskId;
            itemToDb.UserId = userId;

            if (!await _todoTaskRepository.Update(itemToDb))
                throw new NotFoundException($"{taskId} - Not found");

            return itemToDb.Id;
        }

        public async Task<IQueryable<ViewToDoTask>> GetAll()
        {
            var items = await _todoTaskRepository.GetAll();
            
            return items.ProjectTo<ViewToDoTask>(_mapper.ConfigurationProvider);
        }

        public async Task<bool> RemoveFromDb(List<int> ids)
        {
            return await _todoTaskRepository.RemoveFromDb(ids);
        }
    }
}
