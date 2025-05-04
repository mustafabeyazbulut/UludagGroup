using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels;
using UludagGroup.Areas.Finance.ViewModels.OrderViewModels;
using UludagGroup.ViewModels;

namespace UludagGroup.Areas.Finance.Repositories.OrderRepositories
{
    public interface IOrderRepository : IGenericRepository<OrderViewModel, CreateOrderViewModel, UpdateOrderViewModel, OrderViewModel>
    {
        Task<ResponseViewModel<OrderDetailViewModel>> GetAllByOrderIdWithDetailsAsync(int orderId);
        Task<ResponseViewModel<List<OrderDetailViewModel>>> GetAllByCustomerIdWithDetailsAsync(int customerId);
        Task<ResponseViewModel<OrderItemViewModel>> GetByOrderIdAndItemIdAsync(int orderId, int itemId);

    }
}
