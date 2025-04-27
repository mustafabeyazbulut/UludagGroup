using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.CustomerViewModels;
using UludagGroup.Models.Contexts;

namespace UludagGroup.Areas.Finance.Repositories.CustomerRepositories
{
    public class CustomerRepository
        : GenericRepository<CustomerViewModel, CreateCustomerViewModel, UpdateCustomerViewModel, CustomerViewModel>, 
        ICustomerRepository
    {
        public CustomerRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }

}
