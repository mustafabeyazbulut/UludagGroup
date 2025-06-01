using System.ComponentModel.DataAnnotations;

namespace UludagGroup.Areas.Finance.ViewModels.CustomerViewModels
{
    public class UpdateCustomerViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Firma Adı alanı zorunludur.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Müşteri Adı alanı zorunludur.")]
        public string CName { get; set; }
        [Required(ErrorMessage = "Müşteri SoyAdı alanı zorunludur.")]
        public string CSurname { get; set; }

        [Required(ErrorMessage = "Şehir alanı zorunludur.")]
        public string City { get; set; }
        [Required(ErrorMessage = "İlçe alanı zorunludur.")]

        public string District { get; set; }
        [Required(ErrorMessage = "Adres alanı zorunludur.")]

        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^0 \(\d{3}\) \d{3} \d{2} \d{2}$", ErrorMessage = "Telefon formatı geçersiz. Örn: 0 (538) 123 45 67")]
        public string Phone1 { get; set; }

        [RegularExpression(@"^$|^0 \(\d{3}\) \d{3} \d{2} \d{2}$", ErrorMessage = "Telefon formatı geçersiz.")]
        public string Phone2 { get; set; }


        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
        public string Note { get; set; }
    }
}
