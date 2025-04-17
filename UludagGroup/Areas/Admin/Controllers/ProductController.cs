using Microsoft.AspNetCore.Mvc;
using UludagGroup.Commons;
using UludagGroup.Repositories.ProductRepositories;
using UludagGroup.ViewModels.ProductViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _ProductRepo;
        private readonly ImageOperations _imageOperations;

        public ProductController(IProductRepository ProductRepo, ImageOperations imageOperations)
        {
            _ProductRepo = ProductRepo;
            _imageOperations = imageOperations;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _ProductRepo.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateProductViewModel model)
        {
            _imageOperations.FilePath = "Photos/Products";
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                model.ImageUrl = await _imageOperations.UploadImageAsync(model.ImageFile);
            }
            var response = await _ProductRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _ProductRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "Product");
            }
            return View(new UpdateProductViewModel
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                ShortDescription = response.Data.ShortDescription,
                LongDescription = response.Data.LongDescription,
                Price = response.Data.Price,
                ImageUrl = response.Data.ImageUrl,
                Rating = response.Data.Rating,
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateProductViewModel model)
        {
            _imageOperations.FilePath = "Photos/Products";
            var current = await _ProductRepo.GetAsync(model.Id);
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

            var response = await _ProductRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _ProductRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _ProductRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> Feature(int id, bool IsFeatured)
        {
            var response = await _ProductRepo.SetFeaturedStatusAsync(id, !IsFeatured);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Product");
        }
    }
}
