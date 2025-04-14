using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Admin.Controllers
{
    
    public class AdminLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
