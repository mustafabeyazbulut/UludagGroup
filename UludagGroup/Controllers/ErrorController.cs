using Microsoft.AspNetCore.Mvc;
using UludagGroup.ViewModels.ErrorViewModels;

namespace UludagGroup.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult Index()
        {
            var model = new HttpErrorModel
            {
                StatusCode = 500,
                Message = "Beklenmeyen bir hata oluştu."
            };
            return View(model);
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var model = new HttpErrorModel
            {
                StatusCode = statusCode,
                Message = statusCode switch
                {
                    404 => "Sayfa bulunamadı.",
                    403 => "Bu sayfaya erişiminiz yok.",
                    500 => "Sunucu hatası oluştu.",
                    401 => "Erişim için kimlik doğrulaması gerekli.",
                    405 => "Geçersiz HTTP yöntemi.",
                    408 => "İstek zaman aşımına uğradı.",
                    409 => "Çakışan bir durum oluştu.",
                    410 => "Kaynak artık mevcut değil.",
                    429 => "Çok fazla istek gönderildi.",
                    _ => "Bilinmeyen bir hata oluştu."
                }
            };
            return View("Index", model);
        }
    }
}
