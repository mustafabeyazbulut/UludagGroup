using UludagGroup.ViewModels;
using UludagGroup.ViewModels.SocialMediaViewModels;

namespace UludagGroup.Repositories.SocialMediaRepositories
{
    public interface ISocialMediaRepository
    {
        Task<ResponseViewModel<List<SocialMediaViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<SocialMediaViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<SocialMediaViewModel>> GetActiveAsync();
        Task<ResponseViewModel<SocialMediaViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateSocialMediaViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateSocialMediaViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }
}
