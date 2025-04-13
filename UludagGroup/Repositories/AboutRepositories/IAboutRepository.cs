using UludagGroup.ViewModels;
using UludagGroup.ViewModels.AboutViewModels;

namespace UludagGroup.Repositories.AboutRepositories
{
    public interface IAboutRepository
    {
        Task<ResponseViewModel<List<AboutViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<AboutViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<AboutViewModel>> GetActiveAsync();
        Task<ResponseViewModel<AboutViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateAboutViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateAboutViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }
}
