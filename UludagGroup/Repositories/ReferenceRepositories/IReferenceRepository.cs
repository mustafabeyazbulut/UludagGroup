using UludagGroup.ViewModels;
using UludagGroup.ViewModels.ReferenceViewModels;

namespace UludagGroup.Repositories.ReferenceRepositories
{
    public interface IReferenceRepository
    {
        Task<ResponseViewModel<List<ReferenceViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<ReferenceViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<ReferenceViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateReferenceViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateReferenceViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }
}
