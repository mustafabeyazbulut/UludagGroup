using Microsoft.AspNetCore.Mvc;
using UludagGroup.Commons;
using UludagGroup.Repositories.AboutRepositories;
using UludagGroup.ViewModels.AboutViewModels;

namespace UludagGroup.Areas.Admin.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutRepository _aboutRepository;
        private readonly ImageOperations _imageOperations;

        public AboutController(IAboutRepository aboutRepository, ImageOperations imageOperations)
        {
            _aboutRepository = aboutRepository;
            _imageOperations = imageOperations;
            _imageOperations.FilePath = "Photos/Abouts";
        }

        public async Task<IActionResult> Index()
        {
            var response = await _aboutRepository.GetAllAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> SaveAdd(CreateAboutViewModel model)
        {
            _imageOperations.FilePath = "Photos/Abouts";
            if (model.Paragraph1ImageFile!=null && model.Paragraph1ImageFile.Length>0)
            {
                model.Paragraph1Image= await _imageOperations.UploadImageAsync(model.Paragraph1ImageFile);
            }
            if (model.Paragraph2ImageFile != null && model.Paragraph2ImageFile.Length > 0)
            {
                model.Paragraph2Image = await _imageOperations.UploadImageAsync(model.Paragraph2ImageFile);
            }
            if (model.LeftImage1File != null && model.LeftImage1File.Length > 0)
            {
                model.LeftImage1 = await _imageOperations.UploadImageAsync(model.LeftImage1File);
            }
            if (model.LeftImage2File != null && model.LeftImage2File.Length > 0)
            {
                model.LeftImage2 = await _imageOperations.UploadImageAsync(model.LeftImage2File);
            }
            if (model.LeftImage3File != null && model.LeftImage3File.Length > 0)
            {
                model.LeftImage3 = await _imageOperations.UploadImageAsync(model.LeftImage3File);
            }
            if (model.LeftImage4File != null && model.LeftImage4File.Length > 0)
            {
                model.LeftImage4 = await _imageOperations.UploadImageAsync(model.LeftImage4File);
            }
            var response = await _aboutRepository.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Add", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "About");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _aboutRepository.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "About");
            }
            return View(new UpdateAboutViewModel
            {
                Id = response.Data.Id,
                MainTitle = response.Data.MainTitle,
                Paragraph1Image = response.Data.Paragraph1Image,
                Paragraph1Title = response.Data.Paragraph1Title,
                Paragraph1Text = response.Data.Paragraph1Text,
                Paragraph2Image = response.Data.Paragraph2Image,
                Paragraph2Title = response.Data.Paragraph2Title,
                Paragraph2Text = response.Data.Paragraph2Text,
                LeftImage1 = response.Data.LeftImage1,
                LeftImage2 = response.Data.LeftImage2,
                LeftImage3 = response.Data.LeftImage3,
                LeftImage4 = response.Data.LeftImage4
            });
        }
        public async Task<IActionResult> SaveEdit(UpdateAboutViewModel model)
        {
            var current = await _aboutRepository.GetAsync(model.Id);
            if (!current.Status)
            {
                TempData["ErrorMessage"] = $"{current.Message}";
                return View("Edit", model);
            }
            if (model.Paragraph1ImageFile != null && model.Paragraph1ImageFile.Length > 0)
            {
                if(!string.IsNullOrEmpty( current.Data.Paragraph1Image)) 
                    await _imageOperations.DeleteIconAsync(current.Data.Paragraph1Image);
                model.Paragraph1Image = await _imageOperations.UploadImageAsync(model.Paragraph1ImageFile);
            }
            else
            {
                model.Paragraph1Image = current.Data.Paragraph1Image;
            }
            if (model.Paragraph2ImageFile != null && model.Paragraph2ImageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(current.Data.Paragraph2Image))
                    await _imageOperations.DeleteIconAsync(current.Data.Paragraph2Image);
                model.Paragraph2Image = await _imageOperations.UploadImageAsync(model.Paragraph2ImageFile);
            }
            else
            {
                model.Paragraph2Image = current.Data.Paragraph2Image;
            }
            if (model.LeftImage1File != null && model.LeftImage1File.Length > 0)
            {
                if (!string.IsNullOrEmpty(current.Data.LeftImage1))
                    await _imageOperations.DeleteIconAsync(current.Data.LeftImage1);
                model.LeftImage1 = await _imageOperations.UploadImageAsync(model.LeftImage1File);
            }
            else
            {
                model.LeftImage1 = current.Data.LeftImage1;
            }
            if (model.LeftImage2File != null && model.LeftImage2File.Length > 0)
            {
                if (!string.IsNullOrEmpty(current.Data.LeftImage2))
                    await _imageOperations.DeleteIconAsync(current.Data.LeftImage2);
                model.LeftImage2 = await _imageOperations.UploadImageAsync(model.LeftImage2File);
            }
            else
            {
                model.LeftImage2 = current.Data.LeftImage2;
            }
            if (model.LeftImage3File != null && model.LeftImage3File.Length > 0)
            {
                if (!string.IsNullOrEmpty(current.Data.LeftImage3))
                    await _imageOperations.DeleteIconAsync(current.Data.LeftImage3);
                model.LeftImage3 = await _imageOperations.UploadImageAsync(model.LeftImage3File);
            }
            else
            {
                model.LeftImage3 = current.Data.LeftImage3;
            }
            if (model.LeftImage4File != null && model.LeftImage4File.Length > 0)
            {
                if (!string.IsNullOrEmpty(current.Data.LeftImage4))
                    await _imageOperations.DeleteIconAsync(current.Data.LeftImage4);
                model.LeftImage4 = await _imageOperations.UploadImageAsync(model.LeftImage4File);
            }
            else
            {
                model.LeftImage4 = current.Data.LeftImage4;
            }


            var response = await _aboutRepository.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("Edit", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "About");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var response = await _aboutRepository.RemoveAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "About");
        }
        public async Task<IActionResult> Active(int id, bool isActive)
        {
            var response = await _aboutRepository.SetActiveStatusAsync(id, !isActive);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "About");
        }
    }
}
