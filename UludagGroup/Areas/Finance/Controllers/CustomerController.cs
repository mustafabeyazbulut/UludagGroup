using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UludagGroup.Areas.Finance.Repositories.CustomerRepositories;

namespace UludagGroup.Areas.Finance.Controllers
{
    [Authorize(AuthenticationSchemes = "FinanceScheme", Policy = "FinancePolicy")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerController(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<IActionResult> Index()
        {
            //var response = await _customerRepo.GetCustomerDebtInfoAsync();
            //if (!response.Status)
            //{
            //    TempData["ErrorMessage2"] = response.Message;
            //}
            //return View(response.Data);
            return View();
        }
    }
}
