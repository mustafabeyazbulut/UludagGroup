using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    public class ReferenceController : Controller
    {
        public ReferenceController()
        {
        }
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Reference";
            return View();
        }
    }
}
