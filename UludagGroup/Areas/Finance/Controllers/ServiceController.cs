using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Finance.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
