﻿namespace UludagGroup.ViewModels.OurServiceViewModels
{
    public class UpdateOurServiceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Paragraph1 { get; set; }
        public string Paragraph2 { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
