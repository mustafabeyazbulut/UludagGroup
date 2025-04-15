using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Finance.Controllers
{
    [Authorize(AuthenticationSchemes = "FinanceScheme", Policy = "FinancePolicy")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
