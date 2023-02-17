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
        private readonly ICustomListService _customListService;

        public CustomListController(ICustomListService customListResvice)
        {
            _customListService = customListResvice;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IQueryable<ViewCustomList>>> Get()
        {
            var retrivalCustomList = await _customListService.GetByUserId();

            return Ok(retrivalCustomList);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(CreateCustomList list)
        {
            return Ok(await _customListService.Add(list));
        }

        [HttpDelete]
        public async Task<ActionResult<List<int>>> Delete(List<int> ids)
        {
            return Ok(await _customListService.Remove(ids));          
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(CreateCustomList list, int listID)
        {       
            return Ok(await _customListService.Update(list, listID));         
        }
    }
}
