using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Finance.ViewComponents.FinanceLayoutViewCompoents
{
    public class _Head_FinanceLayout_ComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
