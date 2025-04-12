using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
