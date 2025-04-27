using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Auth", new { area = "Admin" });
        }
    }
}
