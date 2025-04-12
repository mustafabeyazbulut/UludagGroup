using UludagGroup.ViewModels;
using UludagGroup.ViewModels.ContactViewModels;

namespace UludagGroup.Repositories.ContactRepositories
{
    public interface IContactRepository
    {
        Task<ResponseViewModel<List<ContactViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<ContactViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<ContactViewModel>> GetActiveAsync();
        Task<ResponseViewModel<ContactViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(CreateContactViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateContactViewModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }
}
