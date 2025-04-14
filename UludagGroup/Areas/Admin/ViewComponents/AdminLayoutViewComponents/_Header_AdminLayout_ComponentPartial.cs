using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _Header_AdminLayout_ComponentPartial : ViewComponent
    {
        public _Header_AdminLayout_ComponentPartial()
        {
        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
