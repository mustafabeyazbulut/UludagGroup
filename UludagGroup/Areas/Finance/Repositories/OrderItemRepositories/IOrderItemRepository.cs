using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels;
using UludagGroup.ViewModels;

namespace UludagGroup.Areas.Finance.Repositories.OrderItemRepositories
{
    public interface IOrderItemRepository : IGenericRepository<OrderItemViewModel, CreateOrderItemViewModel, UpdateOrderItemViewModel, OrderItemViewModel>
    {
        // order id den order item
        Task<ResponseViewModel<List<OrderItemIdViewModel>>> GetAllItemIdsByOrderIdAsync(int orderId);
    }
}
