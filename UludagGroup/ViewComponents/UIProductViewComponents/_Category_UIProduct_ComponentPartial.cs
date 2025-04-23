using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ProductRepositories;
using UludagGroup.ViewModels.ProductViewModels;

namespace UludagGroup.ViewComponents.UIProductViewComponents
{
    public class _Category_UIProduct_ComponentPartial : ViewComponent
    {
        private readonly IProductRepository _productRepository;

        public _Category_UIProduct_ComponentPartial(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(SearchProductViewModel model)
        {
            var response = await _productRepository.GetProductsGroupedByCategoryAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            foreach (var item in response.Data)
            {
                item.IsActive = false;
            }
            
            if (model.CategoryId > 0)
            {
                var category = response.Data.FirstOrDefault(x => x.PGroup == model.CategoryId);
                if (category != null)
                {
                    category.IsActive = true;
                }
            }
            else if (!response.Data.Any(x => x.IsActive))
            {
                var firstItem = response.Data.FirstOrDefault(x => x.PGroup == 0);
                if (firstItem != null)
                {
                    firstItem.IsActive = true;
                }
            }
            return View(response.Data);
        }
    }
}
