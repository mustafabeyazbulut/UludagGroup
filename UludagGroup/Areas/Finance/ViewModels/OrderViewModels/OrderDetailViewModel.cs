using UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels;

namespace UludagGroup.Areas.Finance.ViewModels.OrderViewModels
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CName { get; set; }
        public string CSurname { get; set; }
        public DateTime OrderDate { get; set; }
        public string Notes { get; set; }
        public List<OrderItemDetailViewModel> OrderItems { get; set; }=new List<OrderItemDetailViewModel>();
    }
}
