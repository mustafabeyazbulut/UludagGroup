using Microsoft.AspNetCore.Mvc;
using UludagGroup.Commons;
using UludagGroup.Repositories.FaviconRepositories;
using UludagGroup.ViewModels.FaviconViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class FaviconController : Controller
    {
        private readonly IFaviconRepository _faviconRepo;
        private readonly ImageOperations _imageOperations;
        public FaviconController(IFaviconRepository faviconRepo, ImageOperations imageOperations)
        {
            _faviconRepo = faviconRepo;
            _imageOperations = imageOperations;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _faviconRepo.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateFaviconViewModel model)
        {
            _imageOperations.FilePath = "Photos/Favicons";
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                model.ImageUrl = await _imageOperations.UploadImageAsync(model.ImageFile);
            }
            var response = await _faviconRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Favicon");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _faviconRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "Favicon");
            }
            return View(new UpdateFaviconViewModel
            {
                Id = response.Data.Id,
                Title = response.Data.Title,
                ImageUrl = response.Data.ImageUrl,
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateFaviconViewModel model)
        {
            _imageOperations.FilePath = "Photos/Favicons";
            var current = await _faviconRepo.GetAsync(model.Id);
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

            var response = await _faviconRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Favicon");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _faviconRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Favicon");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _faviconRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Favicon");
        }
    }
}
