using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    public class UILayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
