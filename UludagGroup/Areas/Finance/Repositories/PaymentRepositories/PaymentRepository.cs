using Dapper;
using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.PaymentViewModels;
using UludagGroup.Models.Contexts;
using UludagGroup.ViewModels;

namespace UludagGroup.Areas.Finance.Repositories.PaymentRepositories
{
    public class PaymentRepository
        : GenericRepository<PaymentViewModel, CreatePaymentViewModel, UpdatePaymentViewModel, PaymentViewModel>,
        IPaymentRepository
    {
        public PaymentRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
        public async Task<ResponseViewModel<PaymentDetailViewModel>> GetByIdWithDetailsAsync(int paymentId)
        {
            var response = new ResponseViewModel<PaymentDetailViewModel>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    string query = @"
            SELECT 
                p.Id,
                p.CustomerId,
                p.Amount,
                p.PaymentDate,
                p.Notes,
                c.Name AS CustomerName
            FROM zPayment p
            JOIN zCustomer c ON p.CustomerId = c.Id
            WHERE p.Id = @PaymentId AND p.IsActive = 1
            ";

                    var payment = await connection.QueryFirstOrDefaultAsync<PaymentDetailViewModel>(query, new { PaymentId = paymentId });

                    if (payment == null)
                    {
                        response.Status = false;
                        response.Title = "Hata";
                        response.Message = "Belirtilen ID'ye ait aktif ödeme bulunamadı.";
                        return response;
                    }

                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Veri başarıyla getirildi.";
                    response.Data = payment;
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

        public async Task<ResponseViewModel<List<PaymentDetailViewModel>>> GetAllByCustomerIdWithDetailsAsync(int customerId)
        {
            ResponseViewModel<List<PaymentDetailViewModel>> response = new ResponseViewModel<List<PaymentDetailViewModel>>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    // Müşteriye ait ödemeleri çek
                    string query = @"
                    SELECT 
                        p.Id,
                        p.CustomerId,
                        p.Amount,
                        p.PaymentDate,
                        p.Notes,
                        c.Name AS CustomerName
                    FROM zPayment p
                    JOIN zCustomer c ON p.CustomerId = c.Id
                    WHERE p.CustomerId = @CustomerId AND p.IsActive = 1 
                    Order By p.ID Desc";

                    var payments = (await connection.QueryAsync<PaymentDetailViewModel>(query, new { CustomerId = customerId })).ToList();

                    if (payments == null || !payments.Any())
                    {
                        response.Status = false;
                        response.Title = "Hata";
                        response.Message = "Bu müşteriye ait aktif ödeme bulunamadı.";
                        return response;
                    }
                    response.Status = true;
                    response.Title = "Başarılı";
                    response.Message = "Veriler başarıyla getirildi.";
                    response.Data = payments;
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
