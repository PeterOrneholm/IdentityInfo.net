using Microsoft.AspNetCore.Mvc;

namespace IdentityInfo.Web.Areas.Swedish.Controllers
{
    [Area(SwedishAreaConfiguration.AreaName)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
