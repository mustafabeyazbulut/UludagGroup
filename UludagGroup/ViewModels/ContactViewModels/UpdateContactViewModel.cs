namespace UludagGroup.ViewModels.ContactViewModels
{
    public class UpdateContactViewModel
    {
        public int Id { get; set; }
        public string ContentBody { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string PrimaryAddress { get; set; }
        public string SecondaryAddress { get; set; }
        public string MapUrl { get; set; }
    }
}
