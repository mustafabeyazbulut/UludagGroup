using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.WorkingHourRepositories;
using UludagGroup.ViewModels.WorkingHourViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class WorkingHourController : Controller
    {
        private readonly IWorkingHourRepository _WorkingHourRepo;
        public WorkingHourController(IWorkingHourRepository WorkingHourRepo)
        {
            _WorkingHourRepo = WorkingHourRepo;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _WorkingHourRepo.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateWorkingHourViewModel model)
        {
            var response = await _WorkingHourRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "WorkingHour");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _WorkingHourRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "WorkingHour");
            }
            return View(new UpdateWorkingHourViewModel
            {
                Id = response.Data.Id,
                DayOfWeek = response.Data.DayOfWeek,
                TimeRange = response.Data.TimeRange
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateWorkingHourViewModel model)
        {


            var response = await _WorkingHourRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "WorkingHour");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _WorkingHourRepo.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "WorkingHour");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _WorkingHourRepo.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "WorkingHour");
        }
    }
}
