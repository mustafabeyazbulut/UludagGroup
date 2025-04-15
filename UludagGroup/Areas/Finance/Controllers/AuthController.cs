using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.UserRepositories;

namespace UludagGroup.Areas.Finance.Controllers
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
            // Kullanıcı oturum açmışsa Dashboard'a yönlendir
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(); 
        }
        [HttpPost]
        public async Task<ActionResult> Login(string email, string password, bool rememberMe)
        {
            var response = await _userRepository.FinanceAuthAsync(email, password, rememberMe);
            if (!response.Status)
            {
                ViewBag.ErrorMessage = response.Message;
                return View();
            }
            return RedirectToAction("Dashboard", "Auth");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("FinanceScheme"); // Kullanıcıyı oturumdan çıkarır.
            HttpContext.Session.Clear();      // Session temizlenir.
            return RedirectToAction("Login", "Auth"); // Login sayfasına yönlendirir.
        }
        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View(); // Oturum açmamışsa login ekranını göster
        }
    }
}
