using List_Domain.CreateModel;
using List_Domain.ViewModel;
using List_Service.Interfaces;
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
        public async Task<ActionResult<IQueryable<CustomListView>>> Get()
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value); // НІ
            IQueryable<CustomListView> retrivalCustomList = await _customListResvice.Get(userId);

            return Ok(retrivalCustomList);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(CreateCustomList list)
        {
            try // Запхай в мідлФеа весь ексепш хендлінг, і підключи глобально  https://enlear.academy/global-exception-handling-in-net-6-16908fc8dc28
            {
                var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                return Ok(await _customListResvice.Add(list,userId));
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
        public async Task<ActionResult<List<int>>> Delete(List<int> ids) // нема аейту, нафіга таск?
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value); //АААА

                return Ok(_customListResvice.Remove(ids, userId));
            }
            catch(NotImplementedException)
            {
                return NotFound(ids);
            }
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(CreateCustomList list, int listID)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

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
