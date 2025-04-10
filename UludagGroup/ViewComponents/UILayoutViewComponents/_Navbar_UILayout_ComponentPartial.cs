
using Microsoft.AspNetCore.Mvc;

namespace AlpayMakina.ViewComponents.UILayoutViewComponents
{
    public class _Navbar_UILayout_ComponentPartial:ViewComponent
    {
        //private readonly ISocialMediaRepository _socialMediaRepository;
        //private readonly ICompanyInformationRepository _companyInformationRepository;
        //public _Header_UILayout_ComponentPartial(ISocialMediaRepository socialMediaRepository, 
        //    ICompanyInformationRepository companyInformationRepository)
        //{
        //    _socialMediaRepository = socialMediaRepository;
        //    _companyInformationRepository = companyInformationRepository;
        //}

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    var values= await _socialMediaRepository.GetAllSocialMediaAsync();
        //    var value= await _companyInformationRepository.GetLastCompanyInformationAsync();

        //    ViewBag.CompanyPhone = value.PhoneNumber;
        //    ViewBag.CompanyMail = value.Mail;


        //    return View(values);
        //}
       
        public _Navbar_UILayout_ComponentPartial(  )
        {
          
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
