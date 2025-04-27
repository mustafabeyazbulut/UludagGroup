using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UludagGroup.Areas.Finance.Repositories.CustomerRepositories;

namespace UludagGroup.Areas.Finance.ViewModels.CustomerViewModels
{
    [Authorize(AuthenticationSchemes = "FinanceScheme", Policy = "FinancePolicy")]
    public class FinancialTrackingController : Controller
    {
        private readonly ICustomerRepository _customerRepo;

        public FinancialTrackingController(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _customerRepo.GetCustomerDebtInfoAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
    }
}
