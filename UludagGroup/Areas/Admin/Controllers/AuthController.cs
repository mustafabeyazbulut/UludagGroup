using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.UserRepositories;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(); 
        }
        [HttpPost]
        public async Task<ActionResult> Login(string email, string password, bool rememberMe)
        {
            var response = await _userRepository.AdminAuthAsync(email, password, rememberMe);
            if (!response.Status)
            {
                ViewBag.ErrorMessage = response.Message;
                return View();
            }
            return RedirectToAction("Dashboard", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminScheme"); 
            HttpContext.Session.Clear();      
            return RedirectToAction("Login", "Auth"); 
        }
        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View(); 
        }
    }
}
