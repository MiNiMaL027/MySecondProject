using List_Domain.Models;
using List_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace MySecondProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomListController : Controller
    {
        private readonly ICustomListService _customListResvice;
        public CustomListController(ICustomListService customListResvice)
        {
            _customListResvice = customListResvice;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IQueryable<CustomListDTO>>> Get()
        {
            int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
            IQueryable<CustomListDTO> retrivalCustomList = await _customListResvice.Get(userId);
            return Ok(retrivalCustomList);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(CreateCustomList list)
        {
            try
            {
                int UserId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                return Ok(await _customListResvice.Add(list,UserId));
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

        [HttpDelete]
        public async Task<ActionResult<List<int>>> Delete(List<int> ids)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                return Ok(_customListResvice.Remove(ids, userId));
            }
            catch(NotImplementedException)
            {
                return NotFound(ids);
            }
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(CreateCustomList list,int listID)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                return Ok(await _customListResvice.Update(list, userId,listID));
            }
            catch(NotImplementedException)
            {
                return ValidationProblem();
            }
            catch(NullReferenceException)
            {
                return NotFound(listID);
            }
        }
    }
}
