using List_Domain.CreateModel;
using List_Domain.ModelDTO;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace MySecondProject.Controllers
{
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
        public async Task<ActionResult<IQueryable<ToDoTaskDTO>>> Get()
        {
                int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                IQueryable<ToDoTaskDTO> retrivalToDoTask = await _toDoTaskService.Get(userId);

                return Ok(retrivalToDoTask);
        }
    
        [HttpPost]
        public async Task<ActionResult<int>> Add(CreateToDoTask task)
        {
            try
            {
                int UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                return Ok(await _toDoTaskService.Add(task, UserId));
            }
            catch(NotImplementedException)
            {
                return ValidationProblem();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public async Task<ActionResult<List<int>>> Delete(List<int> ids)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                return Ok(_toDoTaskService.Remove(ids, userId));
            }
            catch(NotImplementedException)
            {
                return NotFound(ids);
            }           
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(CreateToDoTask task, int taskId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                return Ok(await _toDoTaskService.Update(task, userId,taskId));
            }
            catch(NotImplementedException)
            {
                return ValidationProblem();
            }
            catch(NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPut("Complete")]
        public async Task<ActionResult> CompleteOrUncomplete(int id)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                return Ok(await _toDoTaskService.CompleteTask(id, userId));
            }
            catch (NullReferenceException)
            {
                return NotFound(id);
            }
        }
    }
}
