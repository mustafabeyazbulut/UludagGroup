using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.FaviconRepositories;
using UludagGroup.ViewModels.HeadViewModels;

namespace AlpayMakina.ViewComponents.UILayoutViewComponents
{
    public class _Head_UILayout_ComponentPartial : ViewComponent
    {
        private readonly IFaviconRepository _faviconRepository;

        public _Head_UILayout_ComponentPartial(IFaviconRepository faviconRepository)
        {
            _faviconRepository = faviconRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var head = new HeadViewModel();

            var responseFavicon = await _faviconRepository.GetActiveAsync();
            if (responseFavicon.Status)
            {
                head.faviconModel = responseFavicon.Data;
            }
            return View(head);
        }
    }
}
