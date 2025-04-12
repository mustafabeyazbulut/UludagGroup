using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
        ViewData["ActivePage"] = "Contact";
            return View();
        }
    }
}
