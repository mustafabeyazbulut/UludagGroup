using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ProductRepositories;

namespace UludagGroup.ViewComponents.UIProductViewComponents
{
    public class _Featured_UIProduct_ComponentPartial:ViewComponent
    {
        private readonly IProductRepository _productRepository;
        public _Featured_UIProduct_ComponentPartial(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await _productRepository.GetAllActiveFeaturedAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
    }
}
