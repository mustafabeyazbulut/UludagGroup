using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.ServiceViewModels;

namespace UludagGroup.Areas.Finance.Repositories.ServiceRepositories
{
    public interface IServiceRepository: IGenericRepository<ServiceViewModel, CreateServiceViewModel, UpdateServiceViewModel, ServiceViewModel>
    {
    }
}
