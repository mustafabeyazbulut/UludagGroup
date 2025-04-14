using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
