using Microsoft.AspNetCore.Mvc;
using UludagGroup.Areas.Finance.Repositories.OrderRepositories;

namespace UludagGroup.Areas.Finance.ViewComponents.OrderViewComponents
{
    public class _OrderDetail_ComponentPartial : ViewComponent
    {
        private readonly IOrderRepository _orderRepo;

        public _OrderDetail_ComponentPartial( IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var response = await _orderRepo.GetAllByCustomerIdWithDetailsAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
    }
}
