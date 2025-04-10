using Microsoft.AspNetCore.Mvc;

namespace UludagGroup.ViewComponents.UILayoutViewComponents
{
    public class _Footer_UILayout_ComponentPartial:ViewComponent
    {
        //private readonly ICompanyInformationRepository _repository;

        //public _Footer_UILayout_ComponentPartial(ICompanyInformationRepository repository)
        //{
        //    _repository = repository;
        //}

        //public async Task< IViewComponentResult> InvokeAsync()
        //{
        //    var values= await _repository.GetLastCompanyInformationAsync();
        //    return View(values);
        //}
        public _Footer_UILayout_ComponentPartial()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
