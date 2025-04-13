using UludagGroup.ViewModels.ProductViewModels;
using UludagGroup.ViewModels;

namespace UludagGroup.Repositories.ProductRepositories
{
    public interface IProductRepository
    {
        Task<ResponseViewModel<List<ProductViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<ProductViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<List<ProductViewModel>>> GetAllActiveFeaturedAsync();
        Task<ResponseViewModel<ProductViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateProductViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateProductViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
        Task<ResponseViewModel<bool>> SetFeaturedStatusAsync(int id, bool isFeatured);
    }
}
