using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Finance.Controllers
{
    public class FinanceLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
