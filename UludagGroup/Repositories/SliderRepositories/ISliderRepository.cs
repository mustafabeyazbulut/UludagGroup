using UludagGroup.ViewModels;
using UludagGroup.ViewModels.SliderViewModels;

namespace UludagGroup.Repositories.SliderRepositories
{
    public interface ISliderRepository
    {
        Task<ResponseViewModel<List<SliderViewModel>>> GetAllSliderAsync();
        Task<ResponseViewModel<List<SliderViewModel>>> GetAllActiveSliderAsync();
        Task<ResponseViewModel<SliderViewModel>> GetSliderAsync(int id);
        Task<ResponseViewModel<bool>> AddSliderAsync(CreateSliderViewModels model);
        Task<ResponseViewModel<bool>> UpdateSliderAsync(UpdateSliderViewModel model);
        Task<ResponseViewModel<bool>> RemoveSliderAsync(int id);
    }
}
