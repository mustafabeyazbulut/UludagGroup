using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UludagGroup.Repositories.UserRepositories;
using UludagGroup.ViewModels.UserViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _userRepository.GetAllAsync();
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
        public async Task<IActionResult> SaveAdd(CreateUserViewModel model)
        {
            var checkMail = await _userRepository.GetAsync(model.Email);
            if (checkMail.Status)
            {
                TempData["ErrorMessage"] = $"Eklemeye çalıştığınız kullanıcı bulunuyor. Lütfen başka bir kullanıcı ekleyiniz.";
                return RedirectToAction("Add", "User", model);
            }
            var response = await _userRepository.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Add", "User", model);
            }
            return RedirectToAction("Index", "User");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _userRepository.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "User");
            }
            return View(response.Data);
        }
        public async Task<IActionResult> SaveEdit(UpdateUserViewModel model)
        {
            var response = await _userRepository.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "User");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _userRepository.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "User");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _userRepository.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "User");
        }
        public async Task<IActionResult> Admin(int id, bool isAdmin)
        {
            var response = await _userRepository.SetAdminPageAsync(id, !isAdmin);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "User");
        }
        public async Task<IActionResult> Finance(int id, bool isFinance)
        {
            var response = await _userRepository.SetFinancePageAsync(id, !isFinance);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "User");
        }
    }
}
