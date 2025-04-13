using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ReferenceRepositories;

namespace UludagGroup.ViewComponents.ReferenceViewComponents
{
    public class _Main_UIReference_ComponentPartial:ViewComponent
    {
        private readonly IReferenceRepository _referenceRepository;

        public _Main_UIReference_ComponentPartial(IReferenceRepository referenceRepository)
        {
            _referenceRepository = referenceRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
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
