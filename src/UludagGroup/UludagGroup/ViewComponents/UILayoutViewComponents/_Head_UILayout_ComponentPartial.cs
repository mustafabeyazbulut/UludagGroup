using Microsoft.AspNetCore.Mvc;

namespace AlpayMakina.ViewComponents.UILayoutViewComponents
{
    public class _Head_UILayout_ComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
