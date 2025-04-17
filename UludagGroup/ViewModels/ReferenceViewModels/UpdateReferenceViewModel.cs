namespace UludagGroup.ViewModels.ReferenceViewModels
{
    public class UpdateReferenceViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
