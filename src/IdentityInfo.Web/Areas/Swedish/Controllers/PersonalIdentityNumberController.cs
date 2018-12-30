using System.Linq;
using System.Threading.Tasks;
using IdentityInfo.Core.Testdata;
using IdentityInfo.Web.Areas.Swedish.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityInfo.Web.Areas.Swedish.Controllers
{
    [Area(AreaNames.Swedish)]
    [Route("personalidentitynumber")]
    public class PersonalIdentityNumberController : Controller
    {
        private readonly IFlatSwedishPersonalIdentityNumbersTestdataProvider _flatSwedishPersonalIdentityNumbersTestdataProvider;

        public PersonalIdentityNumberController(IFlatSwedishPersonalIdentityNumbersTestdataProvider flatSwedishPersonalIdentityNumbersTestdataProvider)
        {
            _flatSwedishPersonalIdentityNumbersTestdataProvider = flatSwedishPersonalIdentityNumbersTestdataProvider;
        }

        [HttpGet("testdata/list")]
        public async Task<IActionResult> TestDataList()
        {
            var numbers = await _flatSwedishPersonalIdentityNumbersTestdataProvider.GetFlatSwedishPersonalIdentityNumbersAsync();
            var viewModel = new SwedishPersonalIdentityNumberListViewModel(numbers.Take(1000));
            return View(viewModel);
        }
    }
}
