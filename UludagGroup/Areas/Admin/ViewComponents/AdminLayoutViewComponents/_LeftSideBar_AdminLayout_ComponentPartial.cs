using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _LeftSideBar_AdminLayout_ComponentPartial : ViewComponent
    {
        public _LeftSideBar_AdminLayout_ComponentPartial()
        {
        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
