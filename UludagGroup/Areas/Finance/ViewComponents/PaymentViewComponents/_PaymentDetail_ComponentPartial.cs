using Microsoft.AspNetCore.Mvc;
using UludagGroup.Areas.Finance.Repositories.PaymentRepositories;

namespace UludagGroup.Areas.Finance.ViewComponents.PaymentViewComponents
{
    public class _PaymentDetail_ComponentPartial : ViewComponent
    {
        private readonly IPaymentRepository _paymentRepo;

        public _PaymentDetail_ComponentPartial(IPaymentRepository paymentRepo)
        {
            this._paymentRepo = paymentRepo;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var response = await _paymentRepo.GetAllByCustomerIdWithDetailsAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
    }
}
