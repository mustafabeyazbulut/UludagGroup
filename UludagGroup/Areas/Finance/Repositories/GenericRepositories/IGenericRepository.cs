using UludagGroup.ViewModels;

namespace UludagGroup.Areas.Finance.Repositories.GenericRepositories
{
    public interface IGenericRepository<TModel, TCreateModel, TUpdateModel, TViewModel>
    where TViewModel : class, new() // Parametresiz constructor gerekliliğini sağlıyoruz.
    {
        Task<ResponseViewModel<List<TViewModel>>> GetAllAsync();
        Task<ResponseViewModel<List<TViewModel>>> GetAllActiveAsync();
        Task<ResponseViewModel<TViewModel>> GetAsync(int id);
        Task<ResponseViewModel<bool>> AddAsync(TCreateModel model);
        Task<ResponseViewModel<bool>> UpdateAsync(TUpdateModel model);
        Task<ResponseViewModel<bool>> RemoveAsync(int id);
        Task<ResponseViewModel<bool>> SetActiveStatusAsync(int id, bool isActive);
    }

}
