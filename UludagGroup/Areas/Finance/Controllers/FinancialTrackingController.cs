using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using System.Globalization;
using UludagGroup.Areas.Finance.Repositories.CustomerRepositories;
using UludagGroup.Areas.Finance.Repositories.LocationRepostiories;
using UludagGroup.Areas.Finance.Repositories.MailRepositories;
using UludagGroup.Areas.Finance.Repositories.OrderItemRepositories;
using UludagGroup.Areas.Finance.Repositories.OrderRepositories;
using UludagGroup.Areas.Finance.Repositories.PaymentRepositories;
using UludagGroup.Areas.Finance.Repositories.ProductRepositories;
using UludagGroup.Areas.Finance.Repositories.ServiceRepositories;
using UludagGroup.Areas.Finance.ViewModels.CustomerViewModels;
using UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels;
using UludagGroup.Areas.Finance.ViewModels.OrderViewModels;
using UludagGroup.Areas.Finance.ViewModels.PaymentViewModels;
using UludagGroup.Commons;
using UludagGroup.Repositories.ContactRepositories;
using UludagGroup.Repositories.LogoRepositories;

namespace UludagGroup.Areas.Finance.Controllers
{
    [Authorize(AuthenticationSchemes = "FinanceScheme", Policy = "FinancePolicy")]
    public class FinancialTrackingController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly IProductRepository _productRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly ILocationRepostiory _locationRepo;
        private readonly ImageOperations _imageOperations;
        private readonly ILogoRepository _logoRepo;
        private readonly IContactRepository _contactRepository;
        private readonly IMailRepository _mailRepo;

        public FinancialTrackingController(ICustomerRepository customerRepo, IOrderRepository orderRepo, IOrderItemRepository orderItemRepo, IProductRepository productRepo, IServiceRepository serviceRepo, IPaymentRepository paymentRepo, ILocationRepostiory locationRepo, ImageOperations imageOperations, ILogoRepository logoRepo, IContactRepository contactRepository, IMailRepository mailRepo)
        {
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
            this._orderItemRepo = orderItemRepo;
            _productRepo = productRepo;
            _serviceRepo = serviceRepo;
            _paymentRepo = paymentRepo;
            _locationRepo = locationRepo;
            _imageOperations = imageOperations;
            _logoRepo = logoRepo;
            _contactRepository = contactRepository;
            _mailRepo = mailRepo;
        }
        #region Customer
        public async Task<IActionResult> Index()
        {
            var response = await _customerRepo.GetCustomerDebtInfoAsync();
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
        private async Task GetLocations()
        {
            var valuesLocation = await _locationRepo.GetAllAsync();

            List<SelectListItem> locations = valuesLocation.Data
                                             .GroupBy(x => x.City) // Şehre göre grupla
                                             .Select(g => new SelectListItem
                                             {
                                                 Text = g.Key,     // g.Key = City
                                                 Value = g.Key
                                             })
                                             .OrderBy(x => x.Text) // (İsteğe bağlı) alfabetik sırala
                                             .ToList();
            locations.Insert(0, new SelectListItem
            {
                Text = "Şehir Seçebilirsiniz",
                Value = "0"
            });
            ViewBag.Locations = locations;
        }
        private async Task GetLocationsEditCustomer(string city)
        {
            var allLocations = await _locationRepo.GetAllAsync();
            List<SelectListItem> districts = allLocations.Data
        .Where(x => x.City == city)
        .Select(x => x.District)
        .Distinct()
        .Select(d => new SelectListItem
        {
            Text = d,
            Value = d
        })
        .ToList();

            ViewBag.Districts = districts;
            ViewBag.AllDistricts = allLocations.Data;
        }

        [HttpGet]
        public async Task<IActionResult> GetDistrictsByCity(string city)
        {
            var result = await _locationRepo.GetAllAsync();
            var districts = result.Data
                .Where(x => x.City == city)
                .Select(x => x.District)
                .Distinct()
                .ToList();
            return Json(districts);
        }
        public async Task<IActionResult> AddCustomer()
        {
            await GetLocations();
            return View();
        }
        public async Task<IActionResult> SaveAddCustomer(CreateCustomerViewModel model)
        {
            await GetLocations();
            
            var response = await _customerRepo.AddAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("AddCustomer", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Detail", "FinancialTracking", new { id = response.Data });
        }
        public async Task<IActionResult> EditCustomer(int id)
        {
            await GetLocations();
            var response = await _customerRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "FinancialTracking");
            }
            if (!string.IsNullOrEmpty(response.Data.City))
            {
                await GetLocationsEditCustomer(response.Data.City);
            }
            return View(new UpdateCustomerViewModel
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                CName = response.Data.CName,
                CSurname = response.Data.CSurname,
                Email = response.Data.Email,
                City = response.Data.City,
                District = response.Data.District,
                Address = response.Data.Address,
                Phone1 = response.Data.Phone1,
                Phone2 = response.Data.Phone2,
                Note = response.Data.Note
            });
        }
        public async Task<IActionResult> SaveEditCustomer(UpdateCustomerViewModel model)
        {
            await GetLocations();
            var response = await _customerRepo.UpdateAsync(model);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("EditCustomer", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Detail", "FinancialTracking", new { id = model.Id });
        }
        public async Task<IActionResult> RemoveCustomer(int id)
        {
            var response = await _customerRepo.SetActiveStatusAsync(id, false);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Index", "FinancialTracking");
        }
        #endregion
        #region Order
        private async Task GetSelectList()
        {
            var valuesProduct = await _productRepo.GetAllActiveAsync();
            List<SelectListItem> products = (from x in valuesProduct.Data
                                             select new SelectListItem
                                             {
                                                 Text = x.Name,
                                                 Value = x.Id.ToString()
                                             }).ToList();
            products.Insert(0, new SelectListItem
            {
                Text = "Ürün Seçebilirsiniz",
                Value = "0"
            });
            ViewBag.Products = products;
            var valuesService = await _serviceRepo.GetAllActiveAsync();
            List<SelectListItem> services = (from x in valuesService.Data
                                             select new SelectListItem
                                             {
                                                 Text = x.Name,
                                                 Value = x.Id.ToString()
                                             }).ToList();
            services.Insert(0, new SelectListItem
            {
                Text = "Hizmet Seçebilirsiniz",
                Value = "0"
            });
            ViewBag.Services = services;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var customer = await _customerRepo.GetCustomerDebtInfoByIdAsync(id);
            if (!customer.Status)
            {
                TempData["ErrorMessage"] = customer.Message;
                return RedirectToAction("Index", "FinancialTracking");
            }
            return View(customer.Data);
        }
        public async Task<IActionResult> AddOrder(int customerid)
        {
            await GetSelectList();
            var response = await _customerRepo.GetAsync(customerid);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Detail", "FinancialTracking", new { id = customerid });
            }
            return View(new OrderDetailViewModel
            {
                Id = 0,
                CustomerId = response.Data.Id,
                CustomerName = response.Data.Name,
                CName=response.Data.CName,
                CSurname=response.Data.CSurname,
                Notes = "",
                OrderItems = new List<ViewModels.OrderItemViewModels.OrderItemDetailViewModel>()
            });
        }
        public async Task<IActionResult> SaveAddOrder(OrderDetailViewModel model)
        {
            await GetSelectList();
            _imageOperations.FilePath = "Photos/Documents";
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                model.ImageUrl = await _imageOperations.UploadImageAsync(model.ImageFile);
            }
            if (model.Id < 1)
            {
                var orderResponse = await _orderRepo.AddAsync(new CreateOrderViewModel
                {
                    CustomerId = model.CustomerId,
                    Notes = model.Notes,
                    ImageUrl=model.ImageUrl
                });
                if (!orderResponse.Status)
                {
                    TempData["ErrorMessage"] = $"{orderResponse.Message}";
                    return View("AddOrder", model);
                }
                model.Id = orderResponse.Data;
                foreach (var item in model.OrderItems.Where(x=>x.IsVisible))
                {
                    var orderItemResponse = await _orderItemRepo.AddAsync(new CreateOrderItemViewModel
                    {
                        OrderId = orderResponse.Data,
                        ItemId = item.ItemId,
                        ItemType = item.ItemType,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Note = item.Note
                    });
                    if (!orderItemResponse.Status)
                    {
                        TempData["ErrorMessage"] = $"{orderItemResponse.Message}";
                        return View("AddOrder", model);
                    }
                }
            }
            else
            {
                foreach (var item in model.OrderItems)
                {
                    var orderItemResponse = await _orderItemRepo.AddAsync(new CreateOrderItemViewModel
                    {
                        OrderId = model.Id,
                        ItemId = item.ItemId,
                        ItemType = item.ItemType,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Note = item.Note
                    });
                    if (!orderItemResponse.Status)
                    {
                        TempData["ErrorMessage"] = $"{orderItemResponse.Message}";
                        return View("AddOrder", model);
                    }
                }
            }
            return RedirectToAction("Detail", "FinancialTracking", new { id = model.CustomerId });
        }
        public async Task<IActionResult> EditOrder(int id, int customerId)
        {
            var response = await _orderRepo.GetAllByOrderIdWithDetailsAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Detail", "FinancialTracking", new { id = customerId });
            }
            await GetSelectList();

            return View(new OrderDetailViewModel
            {
                Id = response.Data.Id,
                CustomerId = response.Data.CustomerId,
                CustomerName = response.Data.CustomerName,
                CName=response.Data.CName,
                CSurname=response.Data.CSurname,
                Notes = response.Data.Notes,
                OrderDate = response.Data.OrderDate,
                OrderItems = response.Data.OrderItems,
                ImageUrl=response.Data.ImageUrl
            });
        }
        public async Task<IActionResult> SaveEditOrder(OrderDetailViewModel model)
        {
            await GetSelectList();


            _imageOperations.FilePath = "Photos/Documents";
            var current = await _orderRepo.GetAsync(model.Id);
            if (!current.Status)
            {
                TempData["ErrorMessage"] = $"{current.Message}";
                return View("Edit", model);
            }
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(current.Data.ImageUrl))
                    await _imageOperations.DeleteIconAsync(current.Data.ImageUrl);
                model.ImageUrl = await _imageOperations.UploadImageAsync(model.ImageFile);
            }
            else
            {
                model.ImageUrl = current.Data.ImageUrl;
            }


            var update = await _orderRepo.UpdateAsync(new UpdateOrderViewModel
            {
                Id = model.Id,
                CustomerId = model.CustomerId,
                Notes = model.Notes,
                ImageUrl=model.ImageUrl
            });
            if (!update.Status)
            {
                TempData["ErrorMessage"] = $"{update.Message}";
                return View("EditOrder", model);
            }
            foreach (var item in model.OrderItems)
            {
                if (!item.IsVisible)
                {
                    var deleteResponse = await _orderItemRepo.SetActiveStatusAsync(item.Id, false);
                    if (!deleteResponse.Status)
                    {
                        TempData["ErrorMessage"] = $"{deleteResponse.Message}";
                        return View("EditOrder", model);
                    }
                }
                else if (item.Id == 0) // Yeni ekleme
                {
                    var addResponse = await _orderItemRepo.AddAsync(new CreateOrderItemViewModel
                    {
                        OrderId = model.Id,
                        ItemId = item.ItemId,
                        ItemType = item.ItemType,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Note = item.Note
                    });
                    if (!addResponse.Status)
                    {
                        TempData["ErrorMessage"] = $"{addResponse.Message}";
                        return View("EditOrder", model);
                    }
                }
                else if (item.Id > 0) // Güncelleme
                {
                    var updateResponse = await _orderItemRepo.UpdateAsync(new UpdateOrderItemViewModel
                    {
                        Id = item.Id,
                        OrderId = model.Id,
                        ItemId = item.ItemId,
                        ItemType = item.ItemType,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Note = item.Note
                    });
                    if (!updateResponse.Status)
                    {
                        TempData["ErrorMessage"] = $"{updateResponse.Message}";
                        return View("EditOrder", model);
                    }
                }else
                {
                    TempData["ErrorMessage"] = "Geçersiz işlem.";
                    return View("EditOrder", model);
                }
            }
            TempData["SuccessMessage"] = "Sipariş başarıyla güncellendi.";
            return RedirectToAction("Detail", "FinancialTracking", new { id = model.CustomerId });
        }
        public async Task<IActionResult> RemoveOrder(int id, int customerId)
        {
            var orderItemsReponse = await _orderItemRepo.GetAllItemIdsByOrderIdAsync(id);
            foreach (var item in orderItemsReponse.Data)
            {
                await _orderItemRepo.SetActiveStatusAsync(item.Id, false);
            }
            var response = await _orderRepo.SetActiveStatusAsync(id, false);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }

            return RedirectToAction("Detail", "FinancialTracking", new { id = customerId });
        }
        public async Task<IActionResult> RemoveOrderItem(int id, int customerId)
        {
            var response = await _orderItemRepo.SetActiveStatusAsync(id, false);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }

            return RedirectToAction("Detail", "FinancialTracking", new { id = customerId });
        }
        public async Task<IActionResult> SendDocument(int orderId, int customerId)
        {
            // create a new order item for sending document
            var order = await _orderRepo.GetAllByOrderIdWithDetailsAsync(orderId);
            if (!order.Status)
            {
                TempData["ErrorMessage"] = $"{order.Message}";
                return RedirectToAction("Detail", "FinancialTracking", new { id = customerId });
            }


            // create new Service Form  PDF
            using var ms = new MemoryStream();
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var fontRegular = new XFont("Arial", 10, (XFontStyle)0);
            var fontBold = new XFont("Arial", 12, (XFontStyle)1);


            // LOGO
            var responseLogo = await _logoRepo.GetActiveAsync();
            if (responseLogo.Status)
            {
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", "Logos", responseLogo.Data.ImageUrl);
                if (System.IO.File.Exists(logoPath))
                {
                    var image = XImage.FromFile(logoPath);
                    gfx.DrawImage(image, 30, 30, 80, 80);
                }
            }


            var response = await _contactRepository.GetActiveAsync();
            if (response.Status)
            {
                // Firma Bilgileri
                gfx.DrawString("Uludağ Group Çay Kazanları", fontBold, XBrushes.Black, new XPoint(120, 40));
                gfx.DrawString(response.Data.PrimaryAddress, fontRegular, XBrushes.Black, new XPoint(120, 60));
                gfx.DrawString(response.Data.SecondaryAddress, fontRegular, XBrushes.Black, new XPoint(120, 80));
                gfx.DrawString("Telefon:"+response.Data.PrimaryPhone, fontRegular, XBrushes.Black, new XPoint(120, 100));
                gfx.DrawString(response.Data.PrimaryEmail, fontRegular, XBrushes.Black, new XPoint(120, 120));
            }

            // Sağ Üst Bilgiler
            gfx.DrawString("Servis Hizmet Formu", fontBold, XBrushes.Black, new XPoint(page.Width - 180, 40));
            gfx.DrawString($"Tarih: {DateTime.Now:dd.MM.yyyy}", fontRegular, XBrushes.Black, new XPoint(page.Width - 180, 60));
            gfx.DrawString($"Saat: {DateTime.Now:HH:mm}", fontRegular, XBrushes.Black, new XPoint(page.Width - 180, 80));

            // Müşteri Bilgileri Başlığı
            gfx.DrawLine(XPens.Black, 30, 140, page.Width - 30, 140);
            gfx.DrawString("Müşteri Bilgileri", fontBold, XBrushes.Black, new XPoint(30, 160));

            var customer = await _customerRepo.GetAsync(customerId);
            if (customer.Status)
            {
                // Müşteri Bilgileri
                gfx.DrawString($"Firma", fontRegular, XBrushes.Black, new XPoint(30, 180));
                gfx.DrawString($":", fontRegular, XBrushes.Black, new XPoint(80, 180));
                gfx.DrawString($"{customer.Data.Name}", fontRegular, XBrushes.Black, new XPoint(85, 180));

                gfx.DrawString($"Yetkili", fontRegular, XBrushes.Black, new XPoint(30, 200));
                gfx.DrawString($":", fontRegular, XBrushes.Black, new XPoint(80, 200));
                gfx.DrawString($"{customer.Data.CName+" "+customer.Data.CSurname}", fontRegular, XBrushes.Black, new XPoint(85, 200));

                gfx.DrawString($"Telefon", fontRegular, XBrushes.Black, new XPoint(30, 220));
                gfx.DrawString($":", fontRegular, XBrushes.Black, new XPoint(80, 220));
                gfx.DrawString($"{customer.Data.Phone1}", fontRegular, XBrushes.Black, new XPoint(85, 220));


                gfx.DrawString($"Adres", fontRegular, XBrushes.Black, new XRect(30, 230, page.Width - 60, 40), XStringFormats.TopLeft);
                gfx.DrawString($":", fontRegular, XBrushes.Black, new XRect(80, 230, page.Width - 60, 40), XStringFormats.TopLeft);
                gfx.DrawString($"{customer.Data.Address}", fontRegular, XBrushes.Black, new XRect(85, 230, page.Width - 60, 40), XStringFormats.TopLeft);
            }
            gfx.DrawLine(XPens.Black, 30, 250, page.Width - 30, 250);
            gfx.DrawString("Hizmetler", fontBold, XBrushes.Black, new XPoint(30, 280));
            
            // Tablo başlıkları
            int tableX = 30;
            int tableY = 290;
            int rowHeight = 25;
            int tableWidth = (int)page.Width - 60;
            int totalRatio = 8 + 2 + 3 + 3;

            int col1Width = tableWidth * 8/ totalRatio;
            int col2Width = tableWidth * 2 / totalRatio;
            int col3Width = tableWidth * 3 / totalRatio;
            int col4Width = tableWidth - (col1Width + col2Width + col3Width); // kalan, yuvarlama hatasını önler

            // Başlık satırı
            gfx.DrawRectangle(XPens.Black, tableX, tableY, tableWidth, rowHeight);
            gfx.DrawString("Adı", fontRegular, XBrushes.Black, new XRect(tableX+5, tableY, col1Width, rowHeight), XStringFormats.CenterLeft);
            gfx.DrawString("Adet", fontRegular, XBrushes.Black, new XRect(tableX + col1Width, tableY, col2Width, rowHeight), XStringFormats.CenterLeft);
            gfx.DrawString("Birim Fiyat", fontRegular, XBrushes.Black, new XRect(tableX + col1Width + col2Width, tableY, col3Width, rowHeight), XStringFormats.CenterLeft);
            gfx.DrawString("Tutar", fontRegular, XBrushes.Black, new XRect(tableX + col1Width + col2Width + col3Width, tableY, col4Width, rowHeight), XStringFormats.CenterLeft);

            tableY += rowHeight;

            // Hizmetler
            var hizmetler = order.Data.OrderItems.Where(x => x.ItemType == "Service").ToList();
            int maxRows = 5;
            for (int i = 0; i < maxRows; i++)
            {
                var item = i < hizmetler.Count ? hizmetler[i] : null;

                gfx.DrawRectangle(XPens.Gray, tableX, tableY, tableWidth, rowHeight);

                if (item != null)
                {
                    gfx.DrawString(item.ItemName, fontRegular, XBrushes.Black, new XRect(tableX+5, tableY, col1Width, rowHeight), XStringFormats.CenterLeft);
                    gfx.DrawString(item.Quantity.ToString(), fontRegular, XBrushes.Black, new XRect(tableX + col1Width, tableY, col2Width, rowHeight), XStringFormats.CenterLeft);
                    gfx.DrawString(item.UnitPrice.ToString("C", new CultureInfo("tr-TR")), fontRegular, XBrushes.Black, new XRect(tableX + col1Width + col2Width, tableY, col3Width, rowHeight), XStringFormats.CenterLeft);
                    gfx.DrawString(item.LineTotal.ToString("C", new CultureInfo("tr-TR")), fontRegular, XBrushes.Black, new XRect(tableX + col1Width + col2Width + col3Width, tableY, col4Width, rowHeight), XStringFormats.CenterLeft);
                }

                tableY += rowHeight;
            }

            // Genel Toplam
            double pageWidth = gfx.PageSize.Width;
            double margin = 30;
            double totalWidth = pageWidth - 2 * margin;
            double toplamY = tableY + 1;

            // Arka plan (isteğe bağlı)
            gfx.DrawRectangle(XBrushes.LightGray, margin, toplamY, totalWidth, rowHeight-10);

            // Yazılar
            gfx.DrawString("Genel Toplam:", fontBold, XBrushes.Black, new XRect(margin, toplamY, totalWidth - 100, rowHeight-10), XStringFormats.CenterLeft);
            gfx.DrawString(order.Data.OrderItems.Where(x=>x.ItemType== "Service").Sum(x => x.LineTotal).ToString("C", new CultureInfo("tr-TR")), fontBold, XBrushes.Black, new XRect(margin, toplamY, tableX + col1Width + col2Width + col3Width, rowHeight - 10), XStringFormats.CenterRight);



            // Hizmet Notları Başlığı
            toplamY += rowHeight; // bir satır aşağı
            gfx.DrawLine(XPens.Black, margin, toplamY, pageWidth - margin, toplamY); // ayraç çizgi
            toplamY += 18;
            gfx.DrawString("Hizmet Notları", fontBold, XBrushes.Black, new XPoint(margin, toplamY));
            toplamY += 10;

            // En az 5 satır olacak şekilde hizmet notları
            int minNoteRows = 5;
            for (int i = 0; i < minNoteRows; i++)
            {
                string noteText = i < hizmetler.Count ? hizmetler[i]?.Note ?? "" : "";

                gfx.DrawRectangle(XPens.LightGray, margin, toplamY, totalWidth, rowHeight);
                gfx.DrawString($"- {noteText}", fontRegular, XBrushes.Black, new XRect(margin + 5, toplamY + 5, totalWidth - 10, rowHeight), XStringFormats.TopLeft);

                toplamY += rowHeight;
            }

            // Genel Not Başlığı
            toplamY += rowHeight;
            gfx.DrawLine(XPens.Black, margin, toplamY, pageWidth - margin, toplamY);
            toplamY += 18;
            gfx.DrawString("Genel Not", fontBold, XBrushes.Black, new XPoint(margin, toplamY));
            toplamY += 5;

            // Genel Not İçeriği için yaklaşık yükseklik hesaplama
            string genelNot = order.Data.Notes ?? "";
            double maxWidth = totalWidth;
            double lineHeight = fontRegular.GetHeight();
            int approxCharPerLine = (int)(maxWidth / fontRegular.Size * 1.8);
            int lineCount = (int)Math.Ceiling((double)genelNot.Length / approxCharPerLine);
            double neededHeight = lineHeight * lineCount + 10; // biraz padding

            // Genel Not kutusunun çerçevesi
            gfx.DrawRectangle(XPens.Black, margin, toplamY, totalWidth, neededHeight);

            // Metni çizme
            var tf = new XTextFormatter(gfx);
            XRect noteRect = new XRect(margin + 5, toplamY + 5, totalWidth - 10, neededHeight - 10); // iç boşluk için kenarlardan 5 px içeri
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(genelNot, fontRegular, XBrushes.Black, noteRect);

            toplamY += neededHeight + 10;

            // En alt imza alanı için pozisyon ayarları
            double footerMargin = 30;
            double footerY = toplamY ; // Genel Nottan biraz aşağı

            pageWidth = gfx.PageSize.Width;
            double footerWidth = pageWidth - 2 * footerMargin;
            double colWidth = footerWidth / 2;

            // Fontlar (istersen farklı font ve boyut kullanabilirsin)
            XFont fontSignTitle = new XFont("Arial", 10, XFontStyle.Bold);
            XFont fontSignName = new XFont("Arial", 10, XFontStyle.Regular);

            // "TEKNİSYEN" ve "MÜŞTERİ" başlıkları
            gfx.DrawString("TEKNİSYEN", fontSignTitle, XBrushes.Black, new XRect(footerMargin, footerY, colWidth, 20), XStringFormats.TopLeft);
            gfx.DrawString("MÜŞTERİ", fontSignTitle, XBrushes.Black, new XRect(footerMargin + colWidth, footerY, colWidth, 20), XStringFormats.TopLeft);

            footerY += 25;

            // "AD SOYAD      İMZA" satırı için alanlar
            double nameWidth = colWidth * 0.6;
            double signWidth = colWidth * 0.4;

            // Teknisyen bilgileri (örnek, istersen değiştir)
            string teknisyenAdSoyad = "AD SOYAD";
            string teknisyenImza = "İMZA";

            // Müşteri bilgileri (örnek, istersen değiştir)
            string musteriAdSoyad = "AD SOYAD";
            string musteriImza = "İMZA";

            // Teknisyen adı
            gfx.DrawString(teknisyenAdSoyad, fontSignName, XBrushes.Black, new XRect(footerMargin, footerY, nameWidth, 20), XStringFormats.TopLeft);
            // Teknisyen imza
            gfx.DrawString(teknisyenImza, fontSignName, XBrushes.Black, new XRect(footerMargin + nameWidth, footerY, signWidth, 20), XStringFormats.TopLeft);

            // Müşteri adı
            gfx.DrawString(musteriAdSoyad, fontSignName, XBrushes.Black, new XRect(footerMargin + colWidth, footerY, nameWidth, 20), XStringFormats.TopLeft);
            // Müşteri imza
            gfx.DrawString(musteriImza, fontSignName, XBrushes.Black, new XRect(footerMargin + colWidth + nameWidth, footerY, signWidth, 20), XStringFormats.TopLeft);
            // PDF’i döndür
            document.Save(ms, false);
            ms.Position = 0;
            var pdfBytes = ms.ToArray();

            // Mail gönderme
            var mail = await _mailRepo.GetAllAsync();
            if (mail.Status && mail.Data.Count()>0)
            {
                var first = mail.Data.FirstOrDefault()!;
                var mailHelper = new MailHelper(first.Mail, first.Password);

                string htmlBody = @"
                    <html>
                      <body style='font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif; background-color: #f9fafb; padding: 30px;'>
                        <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 30px; box-shadow: 0 4px 12px rgba(0,0,0,0.1);'>
                          <h1 style='color: #2a3f54; font-weight: 700; margin-bottom: 15px;'>Uludağ Çay Kazanları</h1>
                          <h2 style='color: #4a90e2; font-weight: 600;'>Servis Formunuz Hazırlandı!</h2>
      
                          <p style='font-size: 17px; color: #333333; line-height: 1.6;'>
                            Sayın <strong>Değerli Müşterimiz,</strong><br/><br/>
                            Servis talebiniz başarıyla tamamlanmıştır. İlgili servis formu ekte yer almakta olup, incelemeniz için size gönderilmiştir.
                          </p>
      
                          <p style='font-size: 16px; color: #555555; line-height: 1.5;'>
                            Form içeriğinde servis detayları ve yapılan işlemler açıkça belirtilmiştir.<br/>
                            Herhangi bir sorunuz veya ek talebiniz olması durumunda, bizimle iletişime geçmekten çekinmeyiniz.
                          </p>

                          <div style='margin: 25px 0; padding: 15px; background-color: #e9f1fc; border-left: 4px solid #4a90e2;'>
                            <strong style='color: #2a3f54;'>İletişim:</strong><br/>
                            Telefon: <a href='tel:[tel]' style='color: #4a90e2; text-decoration: none;'>+90 XXX XXX XX XX</a><br/>
                            E-posta: <a href='mailto:[mail]' style='color: #4a90e2; text-decoration: none;'>[mail]</a><br/>
                            Web: <a href='https://www.uludagcaykazanlari.com/' style='color: #4a90e2; text-decoration: none;'>www.uludagcaykazanlari.com</a>
                          </div>

                          <p style='font-size: 14px; color: #999999; margin-top: 40px;'>
                            <em>Bu e-posta otomatik olarak gönderilmiştir, lütfen yanıtlamayınız.</em>
                          </p>

                          <p style='font-size: 15px; color: #444444; margin-top: 10px;'>
                            <strong>Uludağ Çay Kazanları Servis Ekibi</strong>
                          </p>
                        </div>
                      </body>
                    </html>
                    ";
                htmlBody = htmlBody.Replace("[tel]", response.Data.PrimaryPhone);
                htmlBody = htmlBody.Replace("[mail]", response.Data.PrimaryEmail);
                bool result = await mailHelper.SendMailWithAttachmentAsync(
                            customer.Data.Email,
                            "Uludağ Çay Kazanları - Servis Formu",
                            htmlBody,
                            pdfBytes,
                            $"ServisFormu_{orderId}.pdf"
                        );
            }

            return File(ms.ToArray(), "application/pdf", $"ServisFormu_{orderId}.pdf");
        }
        #endregion
        #region Payment
        public async Task<IActionResult> AddPayment(int customerid)
        {
            var response = await _customerRepo.GetAsync(customerid);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Detail", "FinancialTracking", new { id = customerid });
            }
            return PartialView("AddPaymentPartial",new PaymentDetailViewModel
            {
                Id = 0,
                CustomerId = response.Data.Id,
                CustomerName = response.Data.Name,
            });
        }
        public async Task<IActionResult> SaveAddPayment(PaymentDetailViewModel model)
        {
            var response = await _paymentRepo.AddAsync(new CreatePaymentViewModel
            {
                Amount = model.Amount,
                CustomerId = model.CustomerId,
                Notes = model.Notes,
                PaymentDate = DateTime.Now
            });
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("AddPayment", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Detail", "FinancialTracking", new { id = model.CustomerId });
        }
        public async Task<IActionResult> EditPayment(int id, int customerId)
        {
            var response = await _paymentRepo.GetByIdWithDetailsAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "FinancialTracking", new { id = customerId });
            }
            return View(response.Data);
        }
        public async Task<IActionResult> SaveEditPayment(PaymentDetailViewModel model)
        {
            var response = await _paymentRepo.UpdateAsync(new UpdatePaymentViewModel
            {
                Amount = model.Amount,
                CustomerId = model.CustomerId,
                Id = model.Id,
                Notes = model.Notes,
                PaymentDate = model.PaymentDate
            });
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return View("EditPayment", model);
            }
            else
            {
                TempData["SuccessMessage"] = $"{response.Message}";
            }
            return RedirectToAction("Detail", "FinancialTracking", new { id = model.CustomerId });
        }
        public async Task<IActionResult> RemovePayment(int id, int customerId)
        {
            var response = await _paymentRepo.SetActiveStatusAsync(id, false);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }

            return RedirectToAction("Detail", "FinancialTracking", new { id = customerId });
        }
        #endregion
    }
}
