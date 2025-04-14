using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.Areas.Finance.ViewComponents.FinanceLayoutViewCompoents
{
    public class _Script_FinanceLayout_ComponentPartial : ViewComponent
    {
        public _Script_FinanceLayout_ComponentPartial()
        {
        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
