using UludagGroup.Areas.Finance.Repositories.GenericRepositories;
using UludagGroup.Areas.Finance.ViewModels.PaymentViewModels;

namespace UludagGroup.Areas.Finance.Repositories.PaymentRepositories
{
    public interface IPaymentRepository : IGenericRepository<PaymentViewModel, CreatePaymentViewModel, UpdatePaymentViewModel, PaymentViewModel>
    {
        // Eğer Payment'a özgü özel metodlar eklemek isterseniz burada tanımlayabilirsiniz.
    }
}
