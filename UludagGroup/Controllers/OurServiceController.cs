using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.OurServiceRepositories;

namespace UludagGroup.Controllers
{
    public class OurServiceController : Controller
    {
        private readonly IOurServiceRepository _ourServiceRepository;

        public OurServiceController(IOurServiceRepository ourServiceRepository)
        {
            _ourServiceRepository = ourServiceRepository;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _ourServiceRepository.GetAllActiveAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            ViewData["ActivePage"] = "OurService";
            return View(response.Data);
        }
    }
}
