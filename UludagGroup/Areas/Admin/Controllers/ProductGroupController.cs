using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ProductGroupRepositories;
using UludagGroup.ViewModels.ProductGroupViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminScheme", Policy = "AdminPolicy")]

    public class ProductGroupController : Controller
    {
        private readonly IProductGroupRepository _productGroupRepo;
        public ProductGroupController(IProductGroupRepository productGroupRepo)
        {
            _productGroupRepo = productGroupRepo;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _productGroupRepo.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateProductGroupViewModel model)
        {
            
            var response = await _productGroupRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "ProductGroup");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _productGroupRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "ProductGroup");
            }
            return View(new UpdateProductGroupViewModel
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateProductGroupViewModel model)
        {
           
            var response = await _productGroupRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "ProductGroup");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _productGroupRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "ProductGroup");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _productGroupRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "ProductGroup");
        }
    }
}
