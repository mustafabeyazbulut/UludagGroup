namespace UludagGroup.ViewModels.AboutViewModels
{
    public class UpdateAboutViewModel
    {
        public int Id { get; set; }
        public string MainTitle { get; set; }
        public string Paragraph1Image { get; set; }
        public string Paragraph1Title { get; set; }
        public string Paragraph1Text { get; set; }
        public string Paragraph2Image { get; set; }
        public string Paragraph2Title { get; set; }
        public string Paragraph2Text { get; set; }
        public string LeftImage1 { get; set; }
        public string LeftImage2 { get; set; }
        public string LeftImage3 { get; set; }
        public string LeftImage4 { get; set; }

        public IFormFile Paragraph1ImageFile { get; set; }
        public IFormFile Paragraph2ImageFile { get; set; }
        public IFormFile LeftImage1File { get; set; }
        public IFormFile LeftImage2File { get; set; }
        public IFormFile LeftImage3File { get; set; }
        public IFormFile LeftImage4File { get; set; }

    }
}
