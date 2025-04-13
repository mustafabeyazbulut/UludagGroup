using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    public class ProductController : Controller
    {

        public ProductController( )
        {
            
        }

        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Product";
            return View();
        }
    }
}
