namespace UludagGroup.ViewModels.LogoViewModels
{
    public class UpdateLogoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
