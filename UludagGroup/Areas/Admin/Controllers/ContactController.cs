using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ContactRepositories;
using UludagGroup.ViewModels.ContactViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepo;
        public ContactController(IContactRepository contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _contactRepo.GetAllAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> SaveAdd(CreateContactViewModel model)
        {
            var response = await _contactRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Contact");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _contactRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "Contact");
            }
            return View(new UpdateContactViewModel
            {
                Id = response.Data.Id,
                ContentBody = response.Data.ContentBody,
                PrimaryEmail = response.Data.PrimaryEmail,
                SecondaryEmail = response.Data.SecondaryEmail,
                PrimaryPhone = response.Data.PrimaryPhone,
                SecondaryPhone = response.Data.SecondaryPhone,
                PrimaryAddress = response.Data.PrimaryAddress,
                SecondaryAddress = response.Data.SecondaryAddress,
                MapUrl = response.Data.MapUrl
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateContactViewModel model)
        {
           

            var response = await _contactRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Contact");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _contactRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Contact");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _contactRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Contact");
        }
    }
}
