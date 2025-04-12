using UludagGroup.ViewModels;
using UludagGroup.ViewModels.SliderViewModels;

namespace UludagGroup.Repositories.SliderRepositories
{
    public interface ISliderRepository
    {
        Task<ResponseViewModel<List<SliderViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<SliderViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<SliderViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateSliderViewModels model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateSliderViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetFirstAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }
}
