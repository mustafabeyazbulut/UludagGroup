using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ProductRepositories;
using UludagGroup.ViewModels.ProductViewModels;

namespace UludagGroup.ViewComponents.UIProductViewComponents
{
    public class _Main_UIProduct_ComponentPartial : ViewComponent
    {
        private readonly IProductRepository _productRepository;

        public _Main_UIProduct_ComponentPartial(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(SearchProductViewModel model)
        {
            var response = await _productRepository.SearchProductsAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
                return View(response.Data); // hata olsa da boş liste dönecek
            }

            var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", "Products");

            foreach (var product in response.Data)
            {
                if (string.IsNullOrWhiteSpace(product.ImageUrl))
                {
                    continue;
                }

                var imagePath = Path.Combine(wwwRootPath, product.ImageUrl);

                if (!System.IO.File.Exists(imagePath))
                {
                    product.ImageUrl = string.Empty;
                }
            }

            return View(response.Data);
        }
    }
}
