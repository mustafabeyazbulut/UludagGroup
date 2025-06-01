using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.LocationViewModels;

namespace UludagGroup.Areas.Finance.Repositories.LocationRepostiories
{
    public interface ILocationRepostiory : IGenericRepository<LocationViewModel, CreateLocationViewModel, UpdateLocationViewModel, LocationViewModel>
    {
    }
}
