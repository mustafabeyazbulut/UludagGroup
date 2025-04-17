using Microsoft.AspNetCore.Mvc;
using UludagGroup.Commons;
using UludagGroup.Repositories.OurServiceRepositories;
using UludagGroup.ViewModels.OurServiceViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class OurServiceController : Controller
    {
        private readonly IOurServiceRepository _ourServiceRepo;
        private readonly ImageOperations _imageOperations;

        public OurServiceController(IOurServiceRepository ourServiceRepo, ImageOperations imageOperations)
        {
            _ourServiceRepo = ourServiceRepo;
            _imageOperations = imageOperations;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _ourServiceRepo.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateOurServiceViewModel model)
        {
            _imageOperations.FilePath = "Photos/OurServices";
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                model.ImageUrl = await _imageOperations.UploadImageAsync(model.ImageFile);
            }
            var response = await _ourServiceRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "OurService");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _ourServiceRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "OurService");
            }
            return View(new UpdateOurServiceViewModel
            {
                Id = response.Data.Id,
                Title = response.Data.Title,
                ImageUrl = response.Data.ImageUrl,
                Paragraph1 = response.Data.Paragraph1,
                Paragraph2 = response.Data.Paragraph2,
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateOurServiceViewModel model)
        {
            _imageOperations.FilePath = "Photos/OurServices";
            var current = await _ourServiceRepo.GetAsync(model.Id);
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

            var response = await _ourServiceRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "OurService");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _ourServiceRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "OurService");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _ourServiceRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "OurService");
        }
    }
}
