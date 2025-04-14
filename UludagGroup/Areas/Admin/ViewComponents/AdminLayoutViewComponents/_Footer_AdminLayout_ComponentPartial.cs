using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _Footer_AdminLayout_ComponentPartial : ViewComponent
    {
        public _Footer_AdminLayout_ComponentPartial()
        {
        }
        public  IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
