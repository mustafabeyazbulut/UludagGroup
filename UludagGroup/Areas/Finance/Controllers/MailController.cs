using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UludagGroup.Areas.Finance.Repositories.MailRepositories;
using UludagGroup.Areas.Finance.ViewModels.MailViewModels;

namespace UludagGroup.Areas.Finance.Controllers
{
    [Authorize(AuthenticationSchemes = "FinanceScheme", Policy = "FinancePolicy")]
    public class MailController : Controller
    {
        private readonly IMailRepository _MailRepo;

        public MailController(IMailRepository MailRepo)
        {
            _MailRepo = MailRepo;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _MailRepo.GetAllAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
        public IActionResult Add()
        {
            return View();
        }
        public async Task<IActionResult> SaveAdd(CreateMailViewModel model)
        {
            var response = await _MailRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Mail");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _MailRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "Mail");
            }
            return View(new UpdateMailViewModel
            {
                Id = response.Data.Id,
                Mail = response.Data.Mail,
                Password = response.Data.Password
               
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateMailViewModel model)
        {
            var response = await _MailRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Mail");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _MailRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Mail");
        }
    }
}
