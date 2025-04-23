using UludagGroup.ViewModels.ProductGroupViewModels;
using UludagGroup.ViewModels;

namespace UludagGroup.Repositories.ProductGroupRepositories
{
    public interface IProductGroupRepository
    {
        Task<ResponseViewModel<List<ProductGroupViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<ProductGroupViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<ProductGroupViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateProductGroupViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateProductGroupViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }
}
