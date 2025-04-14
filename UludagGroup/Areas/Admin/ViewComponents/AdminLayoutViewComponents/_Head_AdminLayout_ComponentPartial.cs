using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _Head_AdminLayout_ComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
