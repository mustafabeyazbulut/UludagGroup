using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.OrderViewModels;
using UludagGroup.Models.Contexts;

namespace UludagGroup.Areas.Finance.Repositories.OrderRepositories
{
    public class OrderRepository : GenericRepository<OrderViewModel, CreateOrderViewModel, UpdateOrderViewModel, OrderViewModel>,
        IOrderRepository
    {
        public OrderRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }
}
