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
                        SELECT 
                            c.Id, 
                            c.Name, 
                            c.Address, 
                            c.Phone, 
                            c.Email,
                            COALESCE(oi.TotalOrderAmount, 0) AS TotalDebt,
                            COALESCE(p.TotalPaidAmount, 0) AS TotalPaid,
                            COALESCE(oi.TotalOrderAmount, 0) - COALESCE(p.TotalPaidAmount, 0) AS RemainingBalance
                        FROM zCustomer c
                        LEFT JOIN (
                            SELECT o.CustomerId, SUM(oi.LineTotal) AS TotalOrderAmount
                            FROM zOrder o
                            INNER JOIN zOrderItem oi ON oi.OrderId = o.Id AND oi.IsActive = 1
                            WHERE o.IsActive = 1
                            GROUP BY o.CustomerId
                        ) oi ON oi.CustomerId = c.Id
                        LEFT JOIN (
                            SELECT p.CustomerId, SUM(p.Amount) AS TotalPaidAmount
                            FROM zPayment p
                            WHERE p.IsActive = 1
                            GROUP BY p.CustomerId
                        ) p ON p.CustomerId = c.Id
                        WHERE c.IsActive = 1;
                        ";
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
        public async Task<ResponseViewModel<CustomerDebtViewModel>> GetCustomerDebtInfoByIdAsync(int customerId)
        {
            var response = new ResponseViewModel<CustomerDebtViewModel>();
            try
            {
                string query = @"
            SELECT 
                c.Id, 
                c.Name, 
                c.Address, 
                c.Phone, 
                c.Email,
                COALESCE(oi.TotalOrderAmount, 0) AS TotalDebt,
                COALESCE(p.TotalPaidAmount, 0) AS TotalPaid,
                COALESCE(oi.TotalOrderAmount, 0) - COALESCE(p.TotalPaidAmount, 0) AS RemainingBalance
            FROM zCustomer c
            LEFT JOIN (
                SELECT o.CustomerId, SUM(oi.LineTotal) AS TotalOrderAmount
                FROM zOrder o
                INNER JOIN zOrderItem oi ON oi.OrderId = o.Id AND oi.IsActive = 1
                WHERE o.IsActive = 1
                GROUP BY o.CustomerId
            ) oi ON oi.CustomerId = c.Id
            LEFT JOIN (
                SELECT p.CustomerId, SUM(p.Amount) AS TotalPaidAmount
                FROM zPayment p
                WHERE p.IsActive = 1
                GROUP BY p.CustomerId
            ) p ON p.CustomerId = c.Id
            WHERE c.IsActive = 1 AND c.Id = @CustomerId;
        ";

                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<CustomerDebtViewModel>(query, new { CustomerId = customerId });

                    if (result != null)
                    {
                        response.Status = true;
                        response.Title = "Başarılı";
                        response.Message = "Müşteri borç bilgisi getirildi.";
                        response.Data = result;
                    }
                    else
                    {
                        response.Status = false;
                        response.Title = "Bulunamadı";
                        response.Message = "Müşteri bulunamadı.";
                    }
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
