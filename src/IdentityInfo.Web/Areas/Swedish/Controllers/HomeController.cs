using Microsoft.AspNetCore.Mvc;

namespace IdentityInfo.Web.Areas.Swedish.Controllers
{
    [Area(AreaNames.Swedish)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
