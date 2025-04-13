using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ContactRepositories;
using UludagGroup.Repositories.FaviconRepositories;
using UludagGroup.Repositories.SocialMediaRepositories;
using UludagGroup.Repositories.WorkingHourRepositories;
using UludagGroup.ViewModels.FooterViewModels;

namespace UludagGroup.ViewComponents.UILayoutViewComponents
{
    public class _Footer_UILayout_ComponentPartial:ViewComponent
    {
        private readonly IFaviconRepository _faviconRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ISocialMediaRepository _socialMediaRepository;
        private readonly IWorkingHourRepository _workingHourRepository;
        public _Footer_UILayout_ComponentPartial(
            IFaviconRepository faviconRepository,
            IContactRepository contactRepository,
            ISocialMediaRepository socialMediaRepository,
            IWorkingHourRepository workingHourRepository)
        {
            _faviconRepository = faviconRepository;
            _contactRepository = contactRepository;
            _socialMediaRepository = socialMediaRepository;
            _workingHourRepository = workingHourRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footer = new FooterViewModel();

            var responseFavicon = await _faviconRepository.GetActiveAsync();
            if (responseFavicon.Status)
            {
                footer.FaviconModel = responseFavicon.Data;
            }
            else
            {
                TempData["ErrorMessage2"] = responseFavicon.Message;
            }
            var responseContact = await _contactRepository.GetActiveAsync();
            if (responseContact.Status)
            {
                footer.ContactModel = responseContact.Data;
            }
            else
            {
                TempData["ErrorMessage2"] =Environment.NewLine+ responseContact.Message;
            }
            var responseSocialMedia = await _socialMediaRepository.GetActiveAsync();
            if (responseSocialMedia.Status)
            {
                footer.SocialMediaModel = responseSocialMedia.Data;
            }
            else
            {
                TempData["ErrorMessage2"] = Environment.NewLine + responseSocialMedia.Message;
            }

            var responseWorkingHour = await _workingHourRepository.GetAllActiveAsync();
            if (responseWorkingHour.Status)
            {
                footer.WorkingHourModel.AddRange(responseWorkingHour.Data);
            }
            else
            {
                TempData["ErrorMessage2"] = Environment.NewLine + responseWorkingHour.Message;
            }
            return View(footer);
        }
    }
}
