using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ReferenceRepositories;

namespace UludagGroup.Controllers
{
    public class ReferenceController : Controller
    {
        private readonly IReferenceRepository _referenceRepository;

        public ReferenceController(IReferenceRepository referenceRepository)
        {
            _referenceRepository = referenceRepository;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _referenceRepository.GetAllActiveAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            ViewData["ActivePage"] = "Reference";
            return View(response.Data);
        }
    }
}
