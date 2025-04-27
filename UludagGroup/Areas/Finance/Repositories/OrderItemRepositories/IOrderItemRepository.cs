using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels;

namespace UludagGroup.Areas.Finance.Repositories.OrderItemRepositories
{
    public interface IOrderItemRepository : IGenericRepository<OrderItemViewModel, CreateOrderItemViewModel, UpdateOrderItemViewModel, OrderItemViewModel>
    {
    }
}
