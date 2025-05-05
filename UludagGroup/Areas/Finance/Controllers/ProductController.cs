using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UludagGroup.Areas.Finance.Repositories.ProductRepositories;
using UludagGroup.Areas.Finance.ViewModels.ProductViewModels;

namespace UludagGroup.Areas.Finance.Controllers
{
    [Authorize(AuthenticationSchemes = "FinanceScheme", Policy = "FinancePolicy")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productRepo.GetAllAsync();
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
            var response = await _productRepo.AddAsync(model);
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
            var response = await _productRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "Product");
            }
            return View(new UpdateProductViewModel
            {
                Id = response.Data.Id,
                Description = response.Data.Description,
                Name = response.Data.Name,
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateProductViewModel model)
        {
            var response = await _productRepo.UpdateAsync(model);
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
            var response = await _productRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _productRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "Product");
        }
    }
}
