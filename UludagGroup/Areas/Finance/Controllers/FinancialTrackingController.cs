using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Threading.Tasks;
using UludagGroup.Areas.Finance.Repositories.CustomerRepositories;
using UludagGroup.Areas.Finance.Repositories.OrderItemRepositories;
using UludagGroup.Areas.Finance.Repositories.OrderRepositories;
using UludagGroup.Areas.Finance.Repositories.PaymentRepositories;
using UludagGroup.Areas.Finance.Repositories.ProductRepositories;
using UludagGroup.Areas.Finance.Repositories.ServiceRepositories;
using UludagGroup.Areas.Finance.ViewModels.CustomerViewModels;
using UludagGroup.Areas.Finance.ViewModels.OrderItemViewModels;
using UludagGroup.Areas.Finance.ViewModels.OrderViewModels;
using UludagGroup.Areas.Finance.ViewModels.PaymentViewModels;

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

        public FinancialTrackingController(ICustomerRepository customerRepo, IOrderRepository orderRepo, IOrderItemRepository orderItemRepo, IProductRepository productRepo, IServiceRepository serviceRepo, IPaymentRepository paymentRepo)
        {
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
            this._orderItemRepo = orderItemRepo;
            _productRepo = productRepo;
            _serviceRepo = serviceRepo;
            _paymentRepo = paymentRepo;
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
        public async Task<IActionResult> AddCustomer()
        {
            return View();
        }
        public async Task<IActionResult> SaveAddCustomer(CreateCustomerViewModel model)
        {
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
            var response = await _customerRepo.GetAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "FinancialTracking");
            }
            return View(new UpdateCustomerViewModel
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                Email = response.Data.Email,
                Address = response.Data.Address,
                Phone = response.Data.Phone
            });
        }
        public async Task<IActionResult> SaveEditCustomer(UpdateCustomerViewModel model)
        {
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
            var response = await _customerRepo.GetAsync(customerid);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Detail", "FinancialTracking", new { id = customerid });
            }
            await GetSelectList();
            return View(new OrderDetailViewModel
            {
                Id = 0,
                CustomerId = response.Data.Id,
                CustomerName = response.Data.Name,
                Notes = "",
                OrderItems = new List<ViewModels.OrderItemViewModels.OrderItemDetailViewModel>()
            });
        }
        public async Task<IActionResult> SaveAddOrder(OrderDetailViewModel model)
        {
            await GetSelectList();
            if (model.Id < 1)
            {
                var orderResponse = await _orderRepo.AddAsync(new CreateOrderViewModel
                {
                    CustomerId = model.CustomerId,
                    Notes = model.Notes
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
                Notes = response.Data.Notes,
                OrderDate = response.Data.OrderDate,
                OrderItems = response.Data.OrderItems
            });
        }
        public async Task<IActionResult> SaveEditOrder(OrderDetailViewModel model)
        {
            await GetSelectList();

            var update = await _orderRepo.UpdateAsync(new UpdateOrderViewModel
            {
                Id = model.Id,
                CustomerId = model.CustomerId,
                Notes = model.Notes
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
            return View(new PaymentDetailViewModel
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
                Method = model.Method,
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
                Method = model.Method,
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
