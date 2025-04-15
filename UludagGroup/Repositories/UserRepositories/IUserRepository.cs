using UludagGroup.ViewModels.UserViewModels;
using UludagGroup.ViewModels;

namespace UludagGroup.Repositories.UserRepositories
{
    public interface IUserRepository
    {
        Task<ResponseViewModel<List<UserViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<UserViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<UserViewModel>> GetAsync(int id);
        Task<ResponseViewModel<UserViewModel>> GetAsync(string email);
        Task<ResponseViewModel<bool>> AddAsync(CreateUserViewModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(UpdateUserViewModel model);
        Task<ResponseViewModel<bool>> UpdatePasswordAsync(int id, string password);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
        Task<ResponseViewModel<bool>> SetAdminPageAsync(int id, bool isAdmin);
        Task<ResponseViewModel<bool>> SetFinancePageAsync(int id, bool isFinance);
        Task<ResponseViewModel<UserViewModel>> FinanceAuthAsync(string email, string password,bool rememberMe);
        Task<ResponseViewModel<UserViewModel>> AdminAuthAsync(string email, string password,bool rememberMe);
    }
}
