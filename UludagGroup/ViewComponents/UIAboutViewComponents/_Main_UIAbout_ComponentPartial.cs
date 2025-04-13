using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.AboutRepositories;

namespace UludagGroup.ViewComponents.UIAboutViewComponents
{
    public class _Main_UIAbout_ComponentPartial : ViewComponent
    {
        private readonly IAboutRepository _aboutRepository;

        public _Main_UIAbout_ComponentPartial(IAboutRepository aboutRepository)
        {
            _aboutRepository = aboutRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await _aboutRepository.GetActiveAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
    }
}
