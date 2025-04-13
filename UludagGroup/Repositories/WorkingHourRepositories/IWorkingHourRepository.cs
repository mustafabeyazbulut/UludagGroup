using UludagGroup.ViewModels;
using UludagGroup.ViewModels.WorkingHourViewModels;

namespace UludagGroup.Repositories.WorkingHourRepositories
{
    public interface IWorkingHourRepository
    {
        Task<ResponseViewModel<List<WorkingHourViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<WorkingHourViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<WorkingHourViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateWorkingHourViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateWorkingHourViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }
}
