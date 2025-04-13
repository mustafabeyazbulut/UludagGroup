using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.OurServiceRepositories;

namespace UludagGroup.ViewComponents.OurServiceViewComponents
{
    public class _Main_UIOurService_ComponentPartial : ViewComponent
    {
        private readonly IOurServiceRepository _ourServiceRepository;

        public _Main_UIOurService_ComponentPartial(IOurServiceRepository ourServiceRepository)
        {
            _ourServiceRepository = ourServiceRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await _ourServiceRepository.GetAllActiveAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
    }
}
