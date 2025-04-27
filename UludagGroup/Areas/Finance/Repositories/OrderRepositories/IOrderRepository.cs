using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.OrderViewModels;

namespace UludagGroup.Areas.Finance.Repositories.OrderRepositories
{
    public interface IOrderRepository : IGenericRepository<OrderViewModel, CreateOrderViewModel, UpdateOrderViewModel, OrderViewModel>
    {
    }
}
