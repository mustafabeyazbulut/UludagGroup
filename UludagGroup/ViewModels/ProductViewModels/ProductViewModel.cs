﻿namespace UludagGroup.ViewModels.ProductViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }
    }
}
