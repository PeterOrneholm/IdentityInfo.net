using System.Linq;
using System.Threading.Tasks;
using IdentityInfo.Core.Swedish.Requests.PersonalIdentityNumbers;
using IdentityInfo.Core.Swedish.Testdata;
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
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //[HttpGet("validate")]
        //public async Task<IActionResult> Validate()
        //{
        //    return View(Core.Swedish.Requests.PersonalIdentityNumbers.Validate.Result.Invalid(string.Empty));
        //}

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
    }
}
