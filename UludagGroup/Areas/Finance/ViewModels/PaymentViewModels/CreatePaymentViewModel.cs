namespace UludagGroup.Areas.Finance.ViewModels.PaymentViewModels
{
    public class CreatePaymentViewModel
    {
        public int CustomerId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }
}
