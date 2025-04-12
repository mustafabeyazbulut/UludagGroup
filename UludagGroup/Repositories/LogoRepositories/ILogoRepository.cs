using UludagGroup.ViewModels;
using UludagGroup.ViewModels.LogoViewModels;

namespace UludagGroup.Repositories.LogoRepositories
{
    public interface ILogoRepository
    {
        Task<ResponseViewModel<List<LogoViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<LogoViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<LogoViewModel>> GetActiveAsync();
        Task<ResponseViewModel<LogoViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateLogoViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateLogoViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }
}
