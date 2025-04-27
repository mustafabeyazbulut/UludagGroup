using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Auth", new { area = "Finance" });
        }
    }
}
