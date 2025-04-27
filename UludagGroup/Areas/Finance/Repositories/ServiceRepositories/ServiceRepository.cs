using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.ServiceViewModels;
using UludagGroup.Models.Contexts;

namespace UludagGroup.Areas.Finance.Repositories.ServiceRepositories
{
    public class ServiceRepository
        : GenericRepository<ServiceViewModel, CreateServiceViewModel, UpdateServiceViewModel, ServiceViewModel>,
        IServiceRepository
    {
        public ServiceRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }
}
