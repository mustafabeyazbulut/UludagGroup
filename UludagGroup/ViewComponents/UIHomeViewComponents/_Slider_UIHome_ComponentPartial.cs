using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.SliderRepositories;

namespace UludagGroup.ViewComponents.UIHomeViewComponents
{
    public class _Slider_UIHome_ComponentPartial : ViewComponent
    {
        private readonly ISliderRepository _sliderRepository;
        public _Slider_UIHome_ComponentPartial(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response =await _sliderRepository.GetAllActiveAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
    }
}
