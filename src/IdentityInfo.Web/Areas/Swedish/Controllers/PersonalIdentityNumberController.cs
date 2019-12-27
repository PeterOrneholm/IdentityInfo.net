using System.Threading.Tasks;
using IdentityInfo.Core.Swedish.Requests.PersonalIdentityNumbers;
using IdentityInfo.Web.Areas.Swedish.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityInfo.Web.Areas.Swedish.Controllers
{
    [Area(SwedishAreaConfiguration.AreaName)]
    [Route("personalidentitynumber")]
    public class PersonalIdentityNumberController : Controller
    {
        private readonly IMediator _meditator;

        public PersonalIdentityNumberController(IMediator meditator)
        {
            _meditator = meditator;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("validate")]
        public async Task<IActionResult> Validate([FromQuery] Validate.Query query)
        {
            if (query.Number != null && query.Number.Length > 50)
            {
                return new BadRequestResult();
            }

            var result = await _meditator.Send(query);
            return View(result);
        }

        [HttpGet("randomize")]
        public async Task<IActionResult> Randomize([FromQuery] Randomize.Query query)
        {
            if (query.Count != null && (query.Count < 1 || query.Count > 10))
            {
                return new BadRequestResult();
            }

            var result = await _meditator.Send(query);
            return View(result);
        }

        [HttpGet("testdata")]
        public async Task<IActionResult> TestDataList([FromQuery] GetTestdataList.Query query)
        {
            if (query.Limit > 1000)
            {
                return new BadRequestResult();
            }

            var result = await _meditator.Send(query);
            var viewModel = new SwedishPersonalIdentityNumberTestdataListViewModel(query, result);
            return View(viewModel);
        }

        [HttpGet("/api/swedish/personalidentitynumber/testdata/json")]
        public async Task<IActionResult> TestDataListApi([FromQuery] GetTestdataList.ApiQuery query)
        {
            var result = await _meditator.Send(query);
            return Json(result);
        }
    }
}
