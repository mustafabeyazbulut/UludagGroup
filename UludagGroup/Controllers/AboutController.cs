using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    public class AboutController : Controller
    {
        public AboutController( )
        {
        }
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "About";
            return View();
        }
    }
}
