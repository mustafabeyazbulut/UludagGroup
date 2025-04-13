using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.FaviconRepositories;
using UludagGroup.ViewModels.CopyrightViewModels;

namespace UludagGroup.ViewComponents.UILayoutViewComponents
{
    public class _Copyright_UILayout_ComponentPartial : ViewComponent
    {
        private readonly IFaviconRepository _faviconRepository;
        public _Copyright_UILayout_ComponentPartial(IFaviconRepository faviconRepository)
        {
            _faviconRepository = faviconRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var copyright = new CopyrightViewModel();
            var responseFavicon = await _faviconRepository.GetActiveAsync();
            if (responseFavicon.Status)
            {
                copyright.FaviconModel = responseFavicon.Data;
            }
            else
            {
                TempData["ErrorMessage2"] = responseFavicon.Message;
            }
            return View(copyright);
        }
    }
}
