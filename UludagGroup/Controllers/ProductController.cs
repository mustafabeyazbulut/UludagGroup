using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ProductRepositories;

namespace UludagGroup.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _productRepository.GetAllActiveAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            ViewData["ActivePage"] = "Product";
            return View(response.Data);
        }
    }
}
