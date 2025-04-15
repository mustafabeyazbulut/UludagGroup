using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminScheme", Policy = "AdminPolicy")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
