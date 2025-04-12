
using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.LogoRepositories;
using UludagGroup.ViewModels.HeadViewModels;
using UludagGroup.ViewModels.NavBarViewModels;

namespace AlpayMakina.ViewComponents.UILayoutViewComponents
{
    public class _Navbar_UILayout_ComponentPartial:ViewComponent
    {
        private readonly ILogoRepository _logoRepository;
        public _Navbar_UILayout_ComponentPartial(ILogoRepository logoRepository)
        {
            _logoRepository = logoRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var head = new NavBarViewModel();

            var responseLogo = await _logoRepository.GetActiveAsync();
            if (responseLogo.Status)
            {
                head.LogoModel = responseLogo.Data;
            }
            return View(head);
        }
    }
}
