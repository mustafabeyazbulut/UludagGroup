namespace UludagGroup.ViewModels.SliderViewModels
{
    public class CreateSliderViewModel
    {
        public string StrongText { get; set; }
        public string NormalText { get; set; }
        public string ContentText { get; set; }
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFirst { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
