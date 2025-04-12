using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["ActivePage"] = "Home";
        return View();
    }
}
