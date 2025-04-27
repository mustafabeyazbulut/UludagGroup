using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.PaymentViewModels;
using UludagGroup.Models.Contexts;

namespace UludagGroup.Areas.Finance.Repositories.PaymentRepositories
{
    public class PaymentRepository
        : GenericRepository<PaymentViewModel, CreatePaymentViewModel, UpdatePaymentViewModel, PaymentViewModel>,
        IPaymentRepository
    {
        public PaymentRepository(Context context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }

}
