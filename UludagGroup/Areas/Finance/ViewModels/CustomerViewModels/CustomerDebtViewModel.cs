namespace UludagGroup.Areas.Finance.ViewModels.CustomerViewModels
{
    public class CustomerDebtViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingBalance { get; set; }
    }
}
