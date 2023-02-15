using AutoMapper;
using List_Dal.Interfaces;
using List_Domain.CreateModel;
using List_Domain.Exeptions;
using List_Domain.Models;
using List_Domain.ViewModel;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace List_Service.Services
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly IToDoTaskRepository _todoTaskRepository;
        private readonly IMapper _mapper;
        private readonly IAutorizationService<ToDoTask> _authService;
        private HttpContext? _httpContext;

        public ToDoTaskService(IToDoTaskRepository toDoTaskRepository, IMapper mapper, IAutorizationService<ToDoTask> authService)
        {
            _mapper = mapper;
            _todoTaskRepository = toDoTaskRepository;
            _authService = authService;
        }


        public void SetHttpContext(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new NotFoundException();

            _httpContext = httpContext;
        }

        public async Task<int> Add(CreateToDoTask item)
        {
            _authService.SetUserId(_httpContext);

            var userId = _authService.GetUserId();

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
            _authService.SetUserId(_httpContext);
            _authService.AuthorizeUser(id);

            return await _todoTaskRepository.CompleteTask(id);
        }

        public async Task<IQueryable<ViewToDoTask>> GetByUserId()
        {
            _authService.SetUserId(_httpContext);

            var userId = _authService.GetUserId();
            var items = await _todoTaskRepository.GetByUser(userId);

            return items.ToList().Select(x => _mapper.Map<ViewToDoTask>(x)).AsQueryable();
        }

        public async Task<List<int>> Remove(List<int> ids)
        {
            _authService.SetUserId(_httpContext);

            foreach (int id in ids)
                _authService.AuthorizeUser(id);

            return await _todoTaskRepository.Remove(ids);
        }

        public async Task<int> Update(CreateToDoTask item, int taskId)
        {
            _authService.SetUserId(_httpContext);
            _authService.AuthorizeUser(taskId);

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
