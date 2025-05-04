using Dapper;
using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels;
using UludagGroup.Areas.Finance.ViewModels.OrderViewModels;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;

namespace UludagGroup.Areas.Finance.Repositories.OrderRepositories
{
    public class OrderRepository : GenericRepository<OrderViewModel, CreateOrderViewModel, UpdateOrderViewModel, OrderViewModel>,
        IOrderRepository
    {
        public OrderRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<ResponseViewModel<List<OrderDetailViewModel>>> GetAllByCustomerIdWithDetailsAsync(int customerId)
        {
            ResponseViewModel<List<OrderDetailViewModel>> response = new ResponseViewModel<List<OrderDetailViewModel>>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    // Önce CustomerId ile aktif siparişleri çekelim
                    string ordersQuery = @"
                                SELECT o.Id, o.CustomerId, o.OrderDate, o.Notes, c.Name AS CustomerName
                                FROM zOrder o
                                JOIN zCustomer c ON o.CustomerId = c.Id
                                WHERE o.CustomerId = @CustomerId AND o.IsActive = 1";

                    var orders = (await connection.QueryAsync<OrderDetailViewModel>(ordersQuery, new { CustomerId = customerId })).ToList();

                    if (orders == null || !orders.Any())
                    {
                        response.Status = false;
                        response.Title = "Hata";
                        response.Message = "Bu müşteriye ait aktif sipariş bulunamadı.";
                        return response;
                    }

                    // Sipariş kalemlerini çekmek için hazırlık
                    string itemsQuery = @"
                                SELECT 
                                    oi.Id, 
                                    oi.OrderId, 
                                    oi.ItemType, 
                                    oi.ItemId, 
                                    COALESCE(s.Name, p.Name) AS ItemName, 
                                    oi.Note, 
                                    oi.UnitPrice, 
                                    oi.Quantity, 
                                    oi.LineTotal, 
                                    oi.IsActive
                                FROM zOrderItem oi
                                LEFT JOIN ZService s ON oi.ItemType = 'Service' AND oi.ItemId = s.Id
                                LEFT JOIN ZProduct p ON oi.ItemType = 'Product' AND oi.ItemId = p.Id
                                WHERE oi.OrderId = @OrderId AND oi.IsActive = 1";

                    // Her sipariş için detayları dolduralım
                    foreach (var order in orders)
                    {
                        var orderItems = (await connection.QueryAsync<OrderItemDetailViewModel>(itemsQuery, new { OrderId = order.Id })).ToList();
                        order.OrderItems = orderItems;
                    }

                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Veriler başarıyla getirildi.";
                    response.Data = orders;
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


        public async Task<ResponseViewModel<OrderDetailViewModel>> GetAllByOrderIdWithDetailsAsync(int orderId)
        {
            ResponseViewModel<OrderDetailViewModel> response = new ResponseViewModel<OrderDetailViewModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    // Orders + Customers JOIN
                    string orderQuery = @"
                                        SELECT o.Id, o.CustomerId, o.OrderDate, o.Notes, c.Name AS CustomerName
                                        FROM zOrder o
                                        JOIN zCustomer c ON o.CustomerId = c.Id
                                        WHERE o.Id = @Id and o.IsActive=1";

                    var order = await connection.QueryFirstOrDefaultAsync<OrderDetailViewModel>(orderQuery, new { Id = orderId });

                    if (order == null)
                    {
                        response.Status = false;
                        response.Title = "Hata";
                        response.Message = "Sipariş bulunamadı.";
                        return response;
                    }

                    // Sipariş kalemlerini çekelim
                    string itemsQuery = @"
                                        SELECT 
                                            oi.Id, 
                                            oi.OrderId, 
                                            oi.ItemType, 
                                            oi.ItemId, 
                                            COALESCE(s.Name, p.Name) AS ItemName, 
                                            oi.Note, 
                                            oi.UnitPrice, 
                                            oi.Quantity, 
                                            oi.LineTotal, 
                                            oi.IsActive
                                        FROM zOrderItem oi
                                        LEFT JOIN ZService s ON oi.ItemType = 'Service' AND oi.ItemId = s.Id
                                        LEFT JOIN ZProduct p ON oi.ItemType = 'Product' AND oi.ItemId = p.Id
                                        WHERE oi.OrderId = @OrderId and oi.IsActive=1";


                    var orderItems = (await connection.QueryAsync<OrderItemDetailViewModel>(itemsQuery, new { OrderId = orderId })).ToList();
                    order.OrderItems = orderItems;

                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Veriler başarıyla getirildi.";
                    response.Data = order;
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



        public Task<ResponseViewModel<OrderItemViewModel>> GetByOrderIdAndItemIdAsync(int orderId, int itemId)
        {
            throw new NotImplementedException();
        }
    }
}
