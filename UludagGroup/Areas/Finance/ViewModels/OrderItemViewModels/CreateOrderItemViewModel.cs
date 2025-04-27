namespace UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels
{
    public class CreateOrderItemViewModel
    {
        public int OrderId { get; set; }
        public string ItemType { get; set; }
        public int ItemId { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
