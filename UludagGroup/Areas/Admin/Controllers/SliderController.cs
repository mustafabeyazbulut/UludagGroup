using Microsoft.AspNetCore.Mvc;
using UludagGroup.Commons;
using UludagGroup.Repositories.SliderRepositories;
using UludagGroup.ViewModels.SliderViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        private readonly ISliderRepository _SliderRepo;
        private readonly ImageOperations _imageOperations;

        public SliderController(ISliderRepository SliderRepo, ImageOperations imageOperations)
        {
            _SliderRepo = SliderRepo;
            _imageOperations = imageOperations;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _SliderRepo.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateSliderViewModel model)
        {
            _imageOperations.FilePath = "Photos/Sliders";
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                model.ImageUrl = await _imageOperations.UploadImageAsync(model.ImageFile);
            }
            var response = await _SliderRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Slider");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _SliderRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "Slider");
            }
            return View(new UpdateSliderViewModel
            {
                Id = response.Data.Id,
                NormalText = response.Data.NormalText,
                StrongText = response.Data.StrongText,
                ContentText = response.Data.ContentText,
                ButtonText = response.Data.ButtonText,
                ButtonLink = response.Data.ButtonLink,
                ImageUrl = response.Data.ImageUrl,
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateSliderViewModel model)
        {
            _imageOperations.FilePath = "Photos/Sliders";
            var current = await _SliderRepo.GetAsync(model.Id);
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

            var response = await _SliderRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Slider");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _SliderRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Slider");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _SliderRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Slider");
        }
        public async Task<IActionResult> First(int id)
        {
            var response = await _SliderRepo.SetFirstAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Slider");
        }
    }
}
