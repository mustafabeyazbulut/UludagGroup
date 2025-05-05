using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Finance.Controllers
{
    [Authorize(AuthenticationSchemes = "FinanceScheme", Policy = "FinancePolicy")]
    public class FinanceLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
