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
    public class HomeController : Controller // НІ НІ НІ, скільки разів говорити?? одна ететя один контроллер, розділити то всьо на хоум ліст і таск контроллери
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
            //так краще
            var model = new ContentStorageModel() 
            {
                _customLists = await _customListService.GetByUserId() 
            };

            ViewBag.NotFound = false;

            return View(model);
        }

        [HttpGet]
        [ActionName("CustomList")]
        public async Task<IActionResult> CustomList(string listName)
        {
            // нема сенсу з цього в трайкетчі
            ViewBag.NotFound = false;
            ViewBag.baseListId = null;
            ViewBag.listName = listName;

            try
            {
                // рядки не мають бути задовгі
                var model = new ContentStorageModel() 
                {
                    _toDoTasks = await _toDoTaskService.GetByListName(listName),
                    _customLists = await _customListService.GetByUserId()
                };

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

                // де ти бачив щоб хтось так в ряд лупашить?) всюди виправи
                var model = new ContentStorageModel() { _toDoTasks = await _toDoTaskService.GetByBaseList(baseListId), _customLists = await _customListService.GetByUserId() };

                return View("HomePage", model);
            }
            catch(NotFoundException)// оце треба винести в загальний фільтр, як в апішці, то точно якось робиться, нагугли ти робиш там редірект на спеціальну Вюшку з 404
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
            // упрости, шукай сам як, багато тупих помилок, якби сів 5 хв подумав то побаичв би
            var lists = await _customListService.GetByUserId();
            var list = lists.FirstOrDefault(x => x.Name == listName);

            task.CustomListId = list.Id;
            
            await _toDoTaskService.Add(task);

            return RedirectToAction("CustomList", new { listName = listName});
        }

        [HttpPost]
        public async Task<IActionResult> Deletetask(int taskId) // треш якийсь, чому не написти просто сервіс.Ремув(Айді)
        {
            var tasks = await _toDoTaskService.GetByUserId();
            var task = tasks.FirstOrDefault(t => t.Id == taskId);

            var currentList = _customListService.GetByUserId().GetAwaiter().GetResult().FirstOrDefault(l => l.Id == task.CustomListId); /// .GetAwaiter().GetResult() ні ні ні, не має так бути

            List<int> currenttask = new List<int> { taskId }; // шо це?? ваааркии

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
