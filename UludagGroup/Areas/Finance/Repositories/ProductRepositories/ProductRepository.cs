using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.ProductViewModels;
using UludagGroup.Models.Contexts;

namespace UludagGroup.Areas.Finance.Repositories.ProductRepositories
{
    public class ProductRepository : GenericRepository<ProductViewModel, CreateProductViewModel, UpdateProductViewModel, ProductViewModel>,
        IProductRepository
    {
        public ProductRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }
   
}
