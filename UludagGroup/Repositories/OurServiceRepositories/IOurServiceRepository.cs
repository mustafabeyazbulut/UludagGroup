using UludagGroup.ViewModels;
using UludagGroup.ViewModels.OurServiceViewModels;

namespace UludagGroup.Repositories.OurServiceRepositories
{
    public interface IOurServiceRepository
    {
        Task<ResponseViewModel<List<OurServiceViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<OurServiceViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<OurServiceViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateOurServiceViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateOurServiceViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }
}
