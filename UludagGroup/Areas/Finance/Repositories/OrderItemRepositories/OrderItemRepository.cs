using Dapper;
using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;

namespace UludagGroup.Areas.Finance.Repositories.OrderItemRepositories
{
    public class OrderItemRepository : GenericRepository<OrderItemViewModel, CreateOrderItemViewModel, UpdateOrderItemViewModel, OrderItemViewModel>,
        IOrderItemRepository
    {
        public OrderItemRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<ResponseViewModel<List<OrderItemIdViewModel>>> GetAllItemIdsByOrderIdAsync(int orderId)
        {
            ResponseViewModel<List<OrderItemIdViewModel>> response = new ResponseViewModel<List<OrderItemIdViewModel>>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    string itemsQuery = @"
                                SELECT 
                                    oi.Id
                                FROM zOrderItem oi
                                WHERE oi.OrderId = @OrderId AND oi.IsActive = 1";

                    var orderItemIds = (await connection.QueryAsync<OrderItemIdViewModel>(itemsQuery, new { OrderId = orderId })).ToList();

                    if (orderItemIds == null || !orderItemIds.Any())
                    {
                        response.Status = false;
                        response.Title = "Hata";
                        response.Message = "Bu siparişe ait öğe bulunamadı.";
                        return response;
                    }
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Öğe ID’leri başarıyla getirildi.";
                    response.Data = orderItemIds.ToList(); // ← dikkat: liste döndürüyoruz
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Title = "Hata";
                response.Message = ex.Message;
            }
            return response;
        }

    }

}
