using UludagGroup.ViewModels.FaviconViewModels;
using UludagGroup.ViewModels;

namespace UludagGroup.Repositories.FaviconRepositories
{
    public interface IFaviconRepository
    {
        Task<ResponseViewModel<List<FaviconViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<FaviconViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<FaviconViewModel>> GetActiveAsync();
        Task<ResponseViewModel<FaviconViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateFaviconViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateFaviconViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }
}
