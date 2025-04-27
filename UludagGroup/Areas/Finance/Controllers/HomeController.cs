using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UludagGroup.Areas.Finance.Repositories.CustomerRepositories;
using UludagGroup.Areas.Finance.Repositories.ProductRepositories;

namespace UludagGroup.Areas.Finance.Controllers
{
    [Authorize(AuthenticationSchemes = "FinanceScheme", Policy = "FinancePolicy")]
    public class HomeController : Controller
    {

        private readonly IProductRepository _customerRepo;

        public HomeController(IProductRepository customerRepo)
        {
            this._customerRepo = customerRepo;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _customerRepo.GetAllActiveAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View();
        }
    }
}
