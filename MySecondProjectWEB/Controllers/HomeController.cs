using List_Domain.CreateModel;
using List_Domain.Exeptions;
using List_Domain.ViewModel;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySecondProjectWEB.Views.ViewHelpsModel;
using System.Collections.Generic;

namespace MySecondProjectWEB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICustomListService _customListService;
        private readonly IToDoTaskService _toDoTaskService;

        public HomeController(ICustomListService customListService, IToDoTaskService toDoTaskService)
        {
            _customListService = customListService;
            _toDoTaskService = toDoTaskService;
        }

        [HttpGet]
        public async Task<IActionResult> HomePage()
        {
            var model = new ContentStorageModel() { _customLists = await _customListService.GetByUserId() };

            ViewBag.NotFound = false;

            return View(model);
        }

        [HttpGet]
        [ActionName("CustomList")]
        public async Task<IActionResult> CustomList(string listName)
        {
            try
            {
                ViewBag.NotFound = false;
                ViewBag.baseListId = null;
                ViewBag.listName = listName;

                var model = new ContentStorageModel() { _toDoTasks = await _toDoTaskService.GetByListName(listName), _customLists = await _customListService.GetByUserId()};

                return View("HomePage", model);
            }
            catch (NotFoundException)
            {
                ViewBag.NotFound = true;

                var model = new ContentStorageModel() { _customLists = await _customListService.GetByUserId() };

                return View("HomePage", model);
            }
        }

        [HttpGet]
        [ActionName("BaseList")]
        public async Task<IActionResult> BaseList(int baseListId)
        {
            try
            {
                ViewBag.NotFound = false;
                ViewBag.listName = null;
                ViewBag.baseListId = baseListId;

                var model = new ContentStorageModel() { _toDoTasks = await _toDoTaskService.GetByBaseList(baseListId), _customLists = await _customListService.GetByUserId() };

                return View("HomePage", model);
            }
            catch(NotFoundException)
            {
                ViewBag.NotFound = true;

                var model = new ContentStorageModel() { _customLists = await _customListService.GetByUserId() };

                return View("HomePage", model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddList(CreateCustomList list)
        {
            await _customListService.Add(list);

            var model = new ContentStorageModel() { _customLists = await _customListService.GetByUserId() };

            return View("HomePage", model);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteList(int listId)
        {
            List<int>  lists = new List<int> { listId };

            await _customListService.Remove(lists);

            var model = new ContentStorageModel() { _customLists = await _customListService.GetByUserId() };

            return View("HomePage", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateList(CreateCustomList list, int listId)
        {
            await _customListService.Update(list, listId);

            var model = new ContentStorageModel() { _customLists = await _customListService.GetByUserId() };

            return View("HomePage", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(CreateToDoTask task, string listName)
        {
            var lists = await _customListService.GetByUserId();
            var list = lists.FirstOrDefault(x => x.Name == listName);

            task.CustomListId = list.Id;
            
            await _toDoTaskService.Add(task);

            return RedirectToAction("CustomList", new { listName = listName});
        }

        [HttpPost]
        public async Task<IActionResult> Deletetask(int taskId)
        {
            var tasks = await _toDoTaskService.GetByUserId();
            var task = tasks.FirstOrDefault(t => t.Id == taskId);

            var currentList = _customListService.GetByUserId().GetAwaiter().GetResult().FirstOrDefault(l => l.Id == task.CustomListId);

            List<int> currenttask = new List<int> { taskId };

            await _toDoTaskService.Remove(currenttask);

            return RedirectToAction("CustomList", new { listName = currentList.Name });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTask(CreateToDoTask task, string listName, int taskId)
        {
            var lists = await _customListService.GetByUserId();
            var list = lists.FirstOrDefaultAsync(x => x.Name == listName);

            task.CustomListId = list.Id;

            await _toDoTaskService.Update(task, taskId);

            var model = new ContentStorageModel() { _customLists = await _customListService.GetByUserId(), _toDoTasks = await _toDoTaskService.GetByListName(listName) };

            return RedirectToAction("CustomList", new { listName = listName });
        }
    }
}
