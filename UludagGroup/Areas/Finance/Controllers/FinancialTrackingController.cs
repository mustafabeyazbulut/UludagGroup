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
                Phone3 = response.Data.Phone3,
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

            using var ms = new MemoryStream();
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var fontRegular = new XFont("Arial", 10, XFontStyle.Regular);
            var fontBold = new XFont("Arial", 12, XFontStyle.Bold);

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
                gfx.DrawString("Uludağ Group Çay Kazanları", fontBold, XBrushes.Black, new XPoint(120, 40));
                gfx.DrawString(response.Data.PrimaryAddress, fontRegular, XBrushes.Black, new XPoint(120, 60));
                gfx.DrawString(response.Data.SecondaryAddress, fontRegular, XBrushes.Black, new XPoint(120, 80));
                gfx.DrawString("Telefon:" + response.Data.PrimaryPhone, fontRegular, XBrushes.Black, new XPoint(120, 100));
                gfx.DrawString(response.Data.PrimaryEmail, fontRegular, XBrushes.Black, new XPoint(120, 120));
            }

            gfx.DrawString("Servis Hizmet Formu", fontBold, XBrushes.Black, new XPoint(page.Width - 180, 40));
            gfx.DrawString($"Tarih: {DateTime.Now:dd.MM.yyyy}", fontRegular, XBrushes.Black, new XPoint(page.Width - 180, 60));
            gfx.DrawString($"Saat: {DateTime.Now:HH:mm}", fontRegular, XBrushes.Black, new XPoint(page.Width - 180, 80));

            // Müşteri Bilgileri Tablosu
            gfx.DrawLine(XPens.Black, 30, 140, page.Width - 30, 140);
            gfx.DrawString("Müşteri Bilgileri", fontBold, XBrushes.Black, new XPoint(30, 160));

            var customer = await _customerRepo.GetAsync(customerId);
            int currentY = 170;
            int rowHeight = 25;
            if (customer.Status)
            {
                void DrawRow(string label, string value)
                {
                    int labelX = 30;
                    int labelWidth = 50;
                    int colonX = labelX + labelWidth; // 80
                    int colonWidth = 10;
                    int valueX = colonX + colonWidth + 5; // 80 + 10 + 5 = 95, yani yazı biraz sağa kayıyor
                    int valueWidth = (int)page.Width - valueX - 30; // sağda biraz boşluk bırak

                    gfx.DrawRectangle(XPens.Black, labelX, currentY, labelWidth, rowHeight);
                    gfx.DrawString(label, fontRegular, XBrushes.Black, new XRect(labelX + 5, currentY, labelWidth - 10, rowHeight), XStringFormats.CenterLeft);

                    gfx.DrawRectangle(XPens.Black, colonX, currentY, colonWidth, rowHeight);
                    gfx.DrawString(":", fontRegular, XBrushes.Black, new XRect(colonX, currentY, colonWidth, rowHeight), XStringFormats.Center);

                    gfx.DrawRectangle(XPens.Black, valueX, currentY, valueWidth, rowHeight);
                    gfx.DrawString(value, fontRegular, XBrushes.Black, new XRect(valueX + 5, currentY, valueWidth - 10, rowHeight), XStringFormats.CenterLeft);

                    currentY += rowHeight + 5;  // satırlar arasında 5 birim boşluk
                }

                DrawRow("Firma", customer.Data.Name);
                DrawRow("Yetkili", $"{customer.Data.CName} {customer.Data.CSurname}");
                DrawRow("Telefon", customer.Data.Phone1);
                DrawRow("Adres", customer.Data.Address);
            }

            // Ürünler
            currentY += 5;
            gfx.DrawLine(XPens.Black, 30, currentY, page.Width - 30, currentY);
            currentY += 15;
            gfx.DrawString("Ürünler", fontBold, XBrushes.Black, new XPoint(30, currentY));
            currentY += 1;

            var urunler = order.Data.OrderItems.Where(x => x.ItemType == "Product").ToList();
            int tableX = 30;
            int tableY = currentY + 5;
            int tableWidth = (int)page.Width - 60;
            int col1 = tableWidth * 6 / 14;
            int col2 = tableWidth * 2 / 14;
            int col3 = tableWidth * 3 / 14;
            int col4 = tableWidth - (col1 + col2 + col3);

            gfx.DrawRectangle(XPens.Black, tableX, tableY, tableWidth, rowHeight);
            gfx.DrawString("Adı", fontRegular, XBrushes.Black, new XRect(tableX + 5, tableY, col1, rowHeight), XStringFormats.CenterLeft);
            gfx.DrawString("Adet", fontRegular, XBrushes.Black, new XRect(tableX + col1, tableY, col2, rowHeight), XStringFormats.CenterLeft);
            gfx.DrawString("Birim Fiyat", fontRegular, XBrushes.Black, new XRect(tableX + col1 + col2, tableY, col3, rowHeight), XStringFormats.CenterLeft);
            gfx.DrawString("Tutar", fontRegular, XBrushes.Black, new XRect(tableX + col1 + col2 + col3, tableY, col4, rowHeight), XStringFormats.CenterLeft);

            tableY += rowHeight;
            int maxRows = 3;
            for (int i = 0; i < maxRows; i++)
            {
                var item = i < urunler.Count ? urunler[i] : null;
                gfx.DrawRectangle(XPens.Gray, tableX, tableY, tableWidth, rowHeight);
                if (item != null)
                {
                    gfx.DrawString(item?.ItemName ?? "", fontRegular, XBrushes.Black, new XRect(tableX + 5, tableY, col1, rowHeight), XStringFormats.CenterLeft);
                    gfx.DrawString(item?.Quantity.ToString() ?? "", fontRegular, XBrushes.Black, new XRect(tableX + col1, tableY, col2, rowHeight), XStringFormats.CenterLeft);
                    gfx.DrawString(item?.UnitPrice.ToString("C", new CultureInfo("tr-TR")) ?? "", fontRegular, XBrushes.Black, new XRect(tableX + col1 + col2, tableY, col3, rowHeight), XStringFormats.CenterLeft);
                    gfx.DrawString(item?.LineTotal.ToString("C", new CultureInfo("tr-TR")) ?? "", fontRegular, XBrushes.Black, new XRect(tableX + col1 + col2 + col3, tableY, col4, rowHeight), XStringFormats.CenterLeft);
                }
                tableY += rowHeight;
            }

            // Hizmetler
            currentY = tableY + 20;
            gfx.DrawString("Hizmetler", fontBold, XBrushes.Black, new XPoint(30, currentY));
            currentY += 5;

            tableY = currentY;
            var hizmetler = order.Data.OrderItems.Where(x => x.ItemType == "Service").ToList();

            gfx.DrawRectangle(XPens.Black, tableX, tableY, tableWidth, rowHeight);
            gfx.DrawString("Adı", fontRegular, XBrushes.Black, new XRect(tableX + 5, tableY, col1, rowHeight), XStringFormats.CenterLeft);
            gfx.DrawString("Adet", fontRegular, XBrushes.Black, new XRect(tableX + col1, tableY, col2, rowHeight), XStringFormats.CenterLeft);
            gfx.DrawString("Birim Fiyat", fontRegular, XBrushes.Black, new XRect(tableX + col1 + col2, tableY, col3, rowHeight), XStringFormats.CenterLeft);
            gfx.DrawString("Tutar", fontRegular, XBrushes.Black, new XRect(tableX + col1 + col2 + col3, tableY, col4, rowHeight), XStringFormats.CenterLeft);

            tableY += rowHeight;
            for (int i = 0; i < maxRows; i++)
            {
                var item = i < hizmetler.Count ? hizmetler[i] : null;
                gfx.DrawRectangle(XPens.Gray, tableX, tableY, tableWidth, rowHeight);
                if (item != null)
                {
                    gfx.DrawString(item?.ItemName ?? "", fontRegular, XBrushes.Black, new XRect(tableX + 5, tableY, col1, rowHeight), XStringFormats.CenterLeft);
                    gfx.DrawString(item?.Quantity.ToString() ?? "", fontRegular, XBrushes.Black, new XRect(tableX + col1, tableY, col2, rowHeight), XStringFormats.CenterLeft);
                    gfx.DrawString(item?.UnitPrice.ToString("C", new CultureInfo("tr-TR")) ?? "", fontRegular, XBrushes.Black, new XRect(tableX + col1 + col2, tableY, col3, rowHeight), XStringFormats.CenterLeft);
                    gfx.DrawString(item?.LineTotal.ToString("C", new CultureInfo("tr-TR")) ?? "", fontRegular, XBrushes.Black, new XRect(tableX + col1 + col2 + col3, tableY, col4, rowHeight), XStringFormats.CenterLeft);
                }
                tableY += rowHeight;
            }

            // Toplam
            currentY = tableY + 5;
            double pageWidth = gfx.PageSize.Width;
            double margin = 30;
            double totalWidth = pageWidth - 2 * margin;
            double toplamY = currentY;

            gfx.DrawRectangle(XBrushes.LightGray, margin, toplamY, totalWidth, rowHeight);
            gfx.DrawString("Genel Toplam:", fontBold, XBrushes.Black, new XRect(margin + 5, toplamY, totalWidth / 2, rowHeight), XStringFormats.CenterLeft);
            gfx.DrawString(order.Data.OrderItems.Where(x => x.ItemType == "Service").Sum(x => x.LineTotal).ToString("C", new CultureInfo("tr-TR")), fontBold, XBrushes.Black, new XRect(margin, toplamY, totalWidth - 10, rowHeight), XStringFormats.CenterRight);

            toplamY += rowHeight + 10;

            toplamY += 10;

            // Hizmet Notları
            gfx.DrawString("Hizmet Notları", fontBold, XBrushes.Black, new XPoint(margin, toplamY));
            toplamY += 5;

            int minNoteRows = 3;
            for (int i = 0; i < minNoteRows; i++)
            {
                string noteText = i < hizmetler.Count ? hizmetler[i]?.Note ?? "" : "";
                gfx.DrawRectangle(XPens.LightGray, margin, toplamY, totalWidth, rowHeight);
                gfx.DrawString("- " + noteText, fontRegular, XBrushes.Black, new XRect(margin + 5, toplamY + 5, totalWidth - 10, rowHeight), XStringFormats.TopLeft);
                toplamY += rowHeight;
            }

            // Genel Not
            toplamY += 20;
            gfx.DrawString("Genel Not", fontBold, XBrushes.Black, new XPoint(margin, toplamY));
            toplamY += 5;

            string genelNot = order.Data.Notes ?? "";
            var tf = new XTextFormatter(gfx);
            var rect = new XRect(margin, toplamY, totalWidth, 40);
            gfx.DrawRectangle(XPens.Black, rect);
            tf.DrawString(genelNot, fontRegular, XBrushes.Black, rect);
            toplamY += 50;

            // İmzalar
            XFont fontSignTitle = new XFont("Arial", 10, XFontStyle.Bold);
            XFont fontSignName = new XFont("Arial", 10, XFontStyle.Regular);
            double colWidth = (page.Width - 2 * margin) / 2;

            gfx.DrawString("TEKNİSYEN", fontSignTitle, XBrushes.Black, new XRect(margin, toplamY, colWidth, 20), XStringFormats.TopLeft);
            gfx.DrawString("MÜŞTERİ", fontSignTitle, XBrushes.Black, new XRect(margin + colWidth, toplamY, colWidth, 20), XStringFormats.TopLeft);
            toplamY += 25;
            gfx.DrawString("AD SOYAD", fontSignName, XBrushes.Black, new XRect(margin, toplamY, colWidth * 0.6, 20), XStringFormats.TopLeft);
            gfx.DrawString("İMZA", fontSignName, XBrushes.Black, new XRect(margin + colWidth * 0.6, toplamY, colWidth * 0.4, 20), XStringFormats.TopLeft);
            gfx.DrawString("AD SOYAD", fontSignName, XBrushes.Black, new XRect(margin + colWidth, toplamY, colWidth * 0.6, 20), XStringFormats.TopLeft);
            gfx.DrawString("İMZA", fontSignName, XBrushes.Black, new XRect(margin + colWidth + colWidth * 0.6, toplamY, colWidth * 0.4, 20), XStringFormats.TopLeft);

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
