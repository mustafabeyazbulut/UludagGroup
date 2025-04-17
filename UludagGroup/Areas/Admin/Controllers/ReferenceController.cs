using Microsoft.AspNetCore.Mvc;
using UludagGroup.Commons;
using UludagGroup.Repositories.ReferenceRepositories;
using UludagGroup.ViewModels.ReferenceViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class ReferenceController : Controller
    {
        private readonly IReferenceRepository _referenceRepo;
        private readonly ImageOperations _imageOperations;
        public ReferenceController(IReferenceRepository referenceRepo, ImageOperations imageOperations)
        {
            _referenceRepo = referenceRepo;
            _imageOperations = imageOperations;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _referenceRepo.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateReferenceViewModel model)
        {
            _imageOperations.FilePath = "Photos/Reference";
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                model.ImageUrl = await _imageOperations.UploadImageAsync(model.ImageFile);
            }
            var response = await _referenceRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Reference");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _referenceRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "Reference");
            }
            return View(new UpdateReferenceViewModel
            {
                Id = response.Data.Id,
                CompanyName = response.Data.CompanyName,
                ImageUrl = response.Data.ImageUrl,
                Description = response.Data.Description
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateReferenceViewModel model)
        {
            _imageOperations.FilePath = "Photos/Reference";
            var current = await _referenceRepo.GetAsync(model.Id);
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

            var response = await _referenceRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Reference");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _referenceRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Reference");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _referenceRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Reference");
        }
    }
}
