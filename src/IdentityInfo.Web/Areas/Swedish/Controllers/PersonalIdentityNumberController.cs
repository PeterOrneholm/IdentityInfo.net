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
        private readonly ISwedishPersonalIdentityNumbersTestdataProvider _swedishPersonalIdentityNumbersTestdataProvider;

        public PersonalIdentityNumberController(ISwedishPersonalIdentityNumbersTestdataProvider swedishPersonalIdentityNumbersTestdataProvider)
        {
            _swedishPersonalIdentityNumbersTestdataProvider = swedishPersonalIdentityNumbersTestdataProvider;
        }

        [HttpGet("testdata/list")]
        public async Task<IActionResult> TestDataList()
        {
            var numbers = await _swedishPersonalIdentityNumbersTestdataProvider.GetSwedishPersonalIdentityNumbersAsync();
            var viewModel = new SwedishPersonalIdentityNumberListViewModel(numbers.Take(100));
            return View(viewModel);
        }
    }
}
