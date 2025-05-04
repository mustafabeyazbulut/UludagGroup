using System.ComponentModel.DataAnnotations;

namespace UludagGroup.Areas.Finance.ViewModels.CustomerViewModels
{
    public class CreateCustomerViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
