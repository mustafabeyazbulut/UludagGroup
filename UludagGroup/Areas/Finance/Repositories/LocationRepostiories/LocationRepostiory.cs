using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.LocationViewModels;
using UludagGroup.Models.Contexts;

namespace UludagGroup.Areas.Finance.Repositories.LocationRepostiories
{
    public class LocationRepostiory : GenericRepository<LocationViewModel, CreateLocationViewModel, UpdateLocationViewModel, LocationViewModel>,
        ILocationRepostiory
    {
        public LocationRepostiory(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }
}
