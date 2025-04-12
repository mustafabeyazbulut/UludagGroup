using Microsoft.AspNetCore.Mvc;
using UludagGroup.ViewModels.PageHeaderViewModels;

namespace UludagGroup.ViewComponents.GenericViewComponents
{
    public class _PageHeader_Generic_ComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke(string pageTitle)
        {
            var controller = ViewContext.RouteData.Values["controller"]?.ToString();
            var action = ViewContext.RouteData.Values["action"]?.ToString();

            var model = new PageHeaderViewModel
            {
                PageTitle = pageTitle,
                Breadcrumb = new List<string> {  controller, action,pageTitle }
            };

            return View(model);
        }
    }
}
