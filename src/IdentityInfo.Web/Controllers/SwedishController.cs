using System.Threading.Tasks;
using IdentityInfo.Core.Testdata;
using Microsoft.AspNetCore.Mvc;
using IdentityInfo.Web.Models;

namespace IdentityInfo.Web.Controllers
{
    public class SwedishController : Controller
    {
        private readonly ISwedishPersonalIdentityNumbersTestdataProvider _swedishPersonalIdentityNumbersTestdataProvider;

        public SwedishController(ISwedishPersonalIdentityNumbersTestdataProvider swedishPersonalIdentityNumbersTestdataProvider)
        {
            _swedishPersonalIdentityNumbersTestdataProvider = swedishPersonalIdentityNumbersTestdataProvider;
        }

        public async Task<IActionResult> PersonalIdentityNumberList()
        {
            var numbers = await _swedishPersonalIdentityNumbersTestdataProvider.GetSwedishPersonalIdentityNumbersAsync();
            var viewModel = new SwedishPersonalIdentityNumberListViewModel(numbers);
            return View(viewModel);
        }
    }
}
