using UludagGroup.ViewModels.ContactViewModels;
using UludagGroup.ViewModels.FaviconViewModels;
using UludagGroup.ViewModels.SocialMediaViewModels;
using UludagGroup.ViewModels.WorkingHourViewModels;

namespace UludagGroup.ViewModels.FooterViewModels
{
    public class FooterViewModel
    {
        public ContactViewModel ContactModel { get; set; }
        public FaviconViewModel FaviconModel { get; set; }
        public SocialMediaViewModel SocialMediaModel { get; set; }
        public List<WorkingHourViewModel> WorkingHourModel { get; set; } = new List<WorkingHourViewModel>();
    }
}
