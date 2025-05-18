using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.PaymentViewModels;
using UludagGroup.ViewModels;

namespace UludagGroup.Areas.Finance.Repositories.PaymentRepositories
{
    public interface IPaymentRepository : IGenericRepository<PaymentViewModel, CreatePaymentViewModel, UpdatePaymentViewModel, PaymentViewModel>
    {
        Task<ResponseViewModel<List<PaymentDetailViewModel>>> GetAllByCustomerIdWithDetailsAsync(int customerId);
        Task<ResponseViewModel<PaymentDetailViewModel>> GetByIdWithDetailsAsync(int paymentId);
    }
}
