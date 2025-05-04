using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UludagGroup.Areas.Finance.Repositories.CustomerRepositories;
using UludagGroup.Areas.Finance.Repositories.OrderItemRepositories;
using UludagGroup.Areas.Finance.Repositories.OrderRepositories;
using UludagGroup.Areas.Finance.Repositories.ProductRepositories;
using UludagGroup.Areas.Finance.Repositories.ServiceRepositories;
using UludagGroup.Areas.Finance.ViewModels.CustomerViewModels;
using UludagGroup.Areas.Finance.ViewModels.OrderViewModels;

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

        public FinancialTrackingController(ICustomerRepository customerRepo, IOrderRepository orderRepo, IOrderItemRepository orderItemRepo, IProductRepository productRepo, IServiceRepository serviceRepo)
        {
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
            this._orderItemRepo = orderItemRepo;
            _productRepo = productRepo;
            _serviceRepo = serviceRepo;
        }

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
            return RedirectToAction("Index", "FinancialTracking");
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
                Name=response.Data.Name,
                Email=response.Data.Email,
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
            return RedirectToAction("Index", "FinancialTracking");
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
       
        public async Task<IActionResult> Detail(int id )
        {
            var customer = await _customerRepo.GetAsync(id);
            if (!customer.Status)
            {
                TempData["ErrorMessage"] = customer.Message;
                return RedirectToAction("Index", "FinancialTracking");
            }
            ViewBag.CustomerName = customer.Data.Name;
            ViewBag.CustomerId = customer.Data.Id;
            var response = await _orderRepo.GetAllByCustomerIdWithDetailsAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage2"] = response.Message;
            }
            return View(response.Data);
        }
        public async Task<IActionResult> AddOrder(int customerid)
        {
            var response = await _customerRepo.GetAsync(customerid);
            //var response = await _orderRepo.GetAllByOrderIdWithDetailsAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "FinancialTracking", new { id = customerid });
            }
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
            //if (!response.Status)
            //{
            //    TempData["ErrorMessage"] = $"{response.Message}";
            //    return View("EditCustomer", model);
            //}
            //else
            //{
            //    TempData["SuccessMessage"] = $"{response.Message}";
            //}
            return RedirectToAction("Detail", "FinancialTracking", new { id = model.CustomerId });
        }
        public async Task<IActionResult> EditOrder(int id,int customerId)
        {
            var response = await _orderRepo.GetAllByOrderIdWithDetailsAsync(id);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
                return RedirectToAction("Index", "FinancialTracking", new { id = customerId });
            }
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
            return View(new OrderDetailViewModel
            {
                Id = response.Data.Id,
                CustomerId=response.Data.CustomerId,
                CustomerName=response.Data.CustomerName,
                Notes = response.Data.Notes,
                OrderDate = response.Data.OrderDate,
                OrderItems = response.Data.OrderItems
            });
        }
        public async Task<IActionResult> SaveEditOrder(OrderDetailViewModel model)
        {
            //if (!response.Status)
            //{
            //    TempData["ErrorMessage"] = $"{response.Message}";
            //    return View("EditCustomer", model);
            //}
            //else
            //{
            //    TempData["SuccessMessage"] = $"{response.Message}";
            //}
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
            var response = await _orderItemRepo.SetActiveStatusAsync(id,false);
            if (!response.Status)
            {
                TempData["ErrorMessage"] = $"{response.Message}";
            }

            return RedirectToAction("Detail", "FinancialTracking", new { id = customerId });
        }
    }
}
