using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Finance.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
