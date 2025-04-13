using Microsoft.AspNetCore.Mvc;
using UludagGroup.Repositories.ContactRepositories;

namespace UludagGroup.ViewComponents.ContactViewComponents
{
    public class _Main_UIContact_ComponentPartial:ViewComponent
    {
        private readonly IContactRepository _contactRepository;
        public _Main_UIContact_ComponentPartial(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await _contactRepository.GetActiveAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
    }
}
