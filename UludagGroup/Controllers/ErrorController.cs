using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
