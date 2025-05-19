namespace UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels
{
    public class OrderItemDetailViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ItemType { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Note { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public bool IsVisible { get; set; } = true;
    }
}
