using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Product";
            return View();
        }
        
        public async Task<IActionResult> Detail(int id)
        {
            var response =await _productRepository.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            ViewData["ActivePage"] = "Product";
            return View(response.Data);
        }
    }
}
