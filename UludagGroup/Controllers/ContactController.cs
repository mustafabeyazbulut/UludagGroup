using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ContactRepositories;

namespace UludagGroup.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IActionResult> Index()
        {
            var response =await _contactRepository.GetActiveAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            ViewData["ActivePage"] = "Contact";
            return View(response.Data);
        }
    }
}
