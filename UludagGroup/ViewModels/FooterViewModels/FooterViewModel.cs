using UludagGroup.ViewModels.ContactViewModels;
using UludagGroup.ViewModels.FaviconViewModels;
using UludagGroup.ViewModels.SocialMediaViewModels;

namespace UludagGroup.ViewModels.FooterViewModels
{
    public class FooterViewModel
    {
        public ContactViewModel ContactModel { get; set; }
        public FaviconViewModel FaviconModel { get; set; }
        public SocialMediaViewModel SocialMediaModel { get; set; }
    }
}
