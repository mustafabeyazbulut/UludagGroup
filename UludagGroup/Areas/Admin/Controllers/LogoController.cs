using Microsoft.AspNetCore.Mvc;
using UludagGroup.Commons;
using UludagGroup.Repositories.LogoRepositories;
using UludagGroup.ViewModels.LogoViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class LogoController : Controller
    {
        private readonly ILogoRepository _logoRepo;
        private readonly ImageOperations _imageOperations;

        public LogoController(ILogoRepository logoRepo, ImageOperations imageOperations)
        {
            _logoRepo = logoRepo;
            _imageOperations = imageOperations;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _logoRepo.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateLogoViewModel model)
        {
            _imageOperations.FilePath = "Photos/Logos";
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                model.ImageUrl = await _imageOperations.UploadImageAsync(model.ImageFile);
            }
            var response = await _logoRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Logo");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _logoRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "Logo");
            }
            return View(new UpdateLogoViewModel
            {
                Id = response.Data.Id,
                Title = response.Data.Title,
                ImageUrl = response.Data.ImageUrl,
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateLogoViewModel model)
        {
            _imageOperations.FilePath = "Photos/Logos";
            var current = await _logoRepo.GetAsync(model.Id);
            if (!current.Status)
            {
                TempData["ErrorMessage"] = $"{current.Message}";
                return View("Edit", model);
            }
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(current.Data.ImageUrl))
                    await _imageOperations.DeleteIconAsync(current.Data.ImageUrl);
                model.ImageUrl = await _imageOperations.UploadImageAsync(model.ImageFile);
            }
            else
            {
                model.ImageUrl = current.Data.ImageUrl;
            }

            var response = await _logoRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Logo");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _logoRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Logo");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _logoRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Logo");
        }
    }
}
