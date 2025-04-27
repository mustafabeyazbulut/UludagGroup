namespace UludagGroup.Areas.Finance.ViewModels.PaymentViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
    }
}
