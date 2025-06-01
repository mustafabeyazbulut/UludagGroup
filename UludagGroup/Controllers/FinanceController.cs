using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers
{
    [Route("Finans")]
    [Route("Finance")]
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Auth", new { area = "Finance" });
        }
    }
}
