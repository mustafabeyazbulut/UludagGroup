namespace UludagGroup.ViewModels.UserViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool IsAdminPage { get; set; }
        public bool IsFinancePage { get; set; }
        public bool IActive { get; set; }
    }
}
