using Dapper;
using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.CustomerViewModels;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;

namespace UludagGroup.Areas.Finance.Repositories.CustomerRepositories
{
    public class CustomerRepository
        : GenericRepository<CustomerViewModel, CreateCustomerViewModel, UpdateCustomerViewModel, CustomerViewModel>,
        ICustomerRepository
    {
        public CustomerRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<ResponseViewModel<List<CustomerDebtViewModel>>> GetCustomerDebtInfoAsync()
        {
            var response = new ResponseViewModel<List<CustomerDebtViewModel>>();
            try
            {
                string query = @"
                        SELECT c.Id, c.Name, c.Address, c.Phone, c.Email, c.Address,
                            -- Toplam borç      
                            COALESCE(SUM(oi.LineTotal), 0) AS TotalDebt,
                            -- Ödenen tutar
                            COALESCE(SUM(p.Amount), 0) AS TotalPaid,
                            -- Kalan borç (Toplam borç - Ödenen tutar)
                            COALESCE(SUM(oi.LineTotal), 0) - COALESCE(SUM(p.Amount), 0) AS RemainingBalance
                        FROM zCustomer c
                        LEFT JOIN zOrder o ON o.CustomerId = c.Id AND o.IsActive = 1 -- Siparişlerin sadece aktif olanlarını al
                        LEFT JOIN zOrderItem oi ON oi.OrderId = o.Id AND oi.IsActive = 1 -- Sipariş kalemlerinin sadece aktif olanlarını al
                        LEFT JOIN zPayment p ON p.CustomerId = c.Id AND p.IsActive = 1 -- Ödemelerin sadece aktif olanlarını al
                        WHERE c.IsActive = 1 -- Müşterilerin sadece aktif olanlarını al
                        GROUP BY c.Id, c.Name, c.Address, c.Phone, c.Email, c.Address;";
                using (var connection = _context.CreateConnection())
                {
                    var values = await connection.QueryAsync<CustomerDebtViewModel>(query);
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Aktif Productlar başarıyla getirildi.";
                    response.Data = values.ToList();
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
