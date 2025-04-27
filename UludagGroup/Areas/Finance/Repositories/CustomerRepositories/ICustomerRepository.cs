using UludagGroup.Areas.Finance.ViewModels.CustomerViewModels;
using UludagGroup.Areas.Finance.Repositories.GenericRepositories;

namespace UludagGroup.Areas.Finance.Repositories.CustomerRepositories
{
    public interface ICustomerRepository
        :  IGenericRepository<CustomerViewModel, CreateCustomerViewModel, UpdateCustomerViewModel, CustomerViewModel>
    {
        // Eğer Customer'a özgü özel metodlar eklemek isterseniz burada tanımlayabilirsiniz.
    }

}
