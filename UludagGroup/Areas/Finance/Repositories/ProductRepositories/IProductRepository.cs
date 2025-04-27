using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.ProductViewModels;

namespace UludagGroup.Areas.Finance.Repositories.ProductRepositories
{
    public interface IProductRepository
        : IGenericRepository<ProductViewModel, CreateProductViewModel, UpdateProductViewModel, ProductViewModel>
    {
        // Eğer Product'a özgü özel metodlar eklemek isterseniz burada tanımlayabilirsiniz.
    }
}
