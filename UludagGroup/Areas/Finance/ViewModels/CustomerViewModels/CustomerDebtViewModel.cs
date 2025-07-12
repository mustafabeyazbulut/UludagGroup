namespace UludagGroup.Areas.Finance.ViewModels.CustomerViewModels
{
    public class CustomerDebtViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CName { get; set; }
        public string CSurname { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Email { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingBalance { get; set; }
        public string Note { get; set; }

    }
}
