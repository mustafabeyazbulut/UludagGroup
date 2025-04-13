using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.AboutRepositories;

namespace UludagGroup.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutRepository _aboutRepository;

        public AboutController(IAboutRepository aboutRepository)
        {
            _aboutRepository = aboutRepository;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _aboutRepository.GetActiveAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            ViewData["ActivePage"] = "About";
            return View(response.Data);
        }
    }
}
