using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.SocialMediaRepositories;
using UludagGroup.ViewModels.SocialMediaViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class SocialMediaController : Controller
    {
        private readonly ISocialMediaRepository _SocialMediaRepo;
        public SocialMediaController(ISocialMediaRepository SocialMediaRepo)
        {
            _SocialMediaRepo = SocialMediaRepo;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _SocialMediaRepo.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateSocialMediaViewModel model)
        {
            var response = await _SocialMediaRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "SocialMedia");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _SocialMediaRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "SocialMedia");
            }
            return View(new UpdateSocialMediaViewModel
            {
                Id = response.Data.Id,
                Twitter = response.Data.Twitter,
                Facebook = response.Data.Facebook,
                Youtube = response.Data.Youtube,
                Linkedin = response.Data.Linkedin,
                Instagram = response.Data.Instagram
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateSocialMediaViewModel model)
        {


            var response = await _SocialMediaRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "SocialMedia");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _SocialMediaRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "SocialMedia");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _SocialMediaRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "SocialMedia");
        }
    }
}
