using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    public class OurServiceController : Controller
    {
        public IActionResult Index()
        {
        ViewData["ActivePage"] = "OurService";
            return View();
        }
    }
}
