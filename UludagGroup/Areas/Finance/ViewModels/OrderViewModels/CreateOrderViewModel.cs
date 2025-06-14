namespace UludagGroup.Areas.Finance.ViewModels.OrderViewModels
{
    public class CreateOrderViewModel
    {
        public int CustomerId { get; set; }
        public string Notes { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
