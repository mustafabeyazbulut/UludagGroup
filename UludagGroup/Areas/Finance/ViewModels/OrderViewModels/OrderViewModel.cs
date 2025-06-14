namespace UludagGroup.Areas.Finance.ViewModels.OrderViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Notes { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
