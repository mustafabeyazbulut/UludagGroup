using UludagGroup.Areas.Finance.ViewModels.CustomerViewModels;
using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.ViewModels;

namespace UludagGroup.Areas.Finance.Repositories.CustomerRepositories
{
    public interface ICustomerRepository
        :  IGenericRepository<CustomerViewModel, CreateCustomerViewModel, UpdateCustomerViewModel, CustomerViewModel>
    {
        Task<ResponseViewModel<List<CustomerDebtViewModel>>> GetCustomerDebtInfoAsync();
    }

}
