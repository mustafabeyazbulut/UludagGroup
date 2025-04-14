using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Finance.ViewComponents.FinanceLayoutViewCompoents
{
    public class _Header_FinanceLayout_ComponentPartial : ViewComponent
    {
        public _Header_FinanceLayout_ComponentPartial()
        {
        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
