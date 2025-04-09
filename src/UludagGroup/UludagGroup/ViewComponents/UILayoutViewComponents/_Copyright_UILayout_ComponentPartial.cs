using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.ViewComponents.UILayoutViewComponents
{
    public class _Copyright_UILayout_ComponentPartial : ViewComponent
    {
        public _Copyright_UILayout_ComponentPartial()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
