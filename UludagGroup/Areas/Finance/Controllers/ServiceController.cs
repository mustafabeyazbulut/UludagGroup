using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UludagGroup.Areas.Finance.Repositories.ServiceRepositories;
using UludagGroup.Areas.Finance.ViewModels.ServiceViewModels;

namespace UludagGroup.Areas.Finance.Controllers
{
    [Authorize(AuthenticationSchemes = "FinanceScheme", Policy = "FinancePolicy")]
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepo;

        public ServiceController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _serviceRepo.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateServiceViewModel model)
        {
            var response = await _serviceRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "service");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _serviceRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "service");
            }
            return View(new UpdateServiceViewModel
            {
                Id = response.Data.Id,
                Description = response.Data.Description,
                Name= response.Data.Name,
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateServiceViewModel model)
        {
            var response = await _serviceRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "service");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _serviceRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "service");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _serviceRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "service");
        }
    }
}
