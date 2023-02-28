using List_Domain.CreateModel;
using List_Domain.ViewModel;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Runtime.CompilerServices;

namespace MySecondProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTaskController : Controller
    {
        private readonly IToDoTaskService _toDoTaskService;

        public ToDoTaskController(IToDoTaskService toDoTaskService)
        {
            _toDoTaskService = toDoTaskService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IQueryable<ViewToDoTask>>> Get()
        {
            IQueryable<ViewToDoTask> retrivalToDoTask = await _toDoTaskService.GetByUserId();

            return Ok(retrivalToDoTask);
        }

        [HttpGet("byListName")]
        public async Task<ActionResult<List<ViewToDoTask>>> GetByListName(string listName)
        {
            return Ok(await _toDoTaskService.GetByListName(listName));
        }
    
        [HttpPost]
        public async Task<ActionResult<int>> Add(CreateToDoTask task)
        {
            return Ok(await _toDoTaskService.Add(task));
        }

        [HttpDelete]
        public async Task<ActionResult<List<int>>> Delete(List<int> ids)
        {
            return Ok(await _toDoTaskService.Remove(ids));         
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(CreateToDoTask task, int taskId)
        {
            return Ok(await _toDoTaskService.Update(task, taskId));      
        }

        [HttpPut("Complete")]
        public async Task<ActionResult> CompleteOrUncomplete(int id)
        {          
            return Ok(await _toDoTaskService.CompleteTask(id));
        }
    }
}
