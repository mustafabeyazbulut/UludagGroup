namespace UludagGroup.Areas.Finance.ViewModels.PaymentViewModels
{
    public class PaymentDetailViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }
}
