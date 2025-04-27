using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels;
using UludagGroup.Models.Contexts;

namespace UludagGroup.Areas.Finance.Repositories.OrderItemRepositories
{
    public class OrderItemRepository : GenericRepository<OrderItemViewModel, CreateOrderItemViewModel, UpdateOrderItemViewModel, OrderItemViewModel>,
        IOrderItemRepository
    {
        public OrderItemRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }

}
