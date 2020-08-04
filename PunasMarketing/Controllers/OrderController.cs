using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Order;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PunasMarketing.Models.Enums;

namespace PunasMarketing.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepo;
        private readonly OrderItemRepository _orderItemRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly PersonnelRepository _personnelRepo;
        private readonly ProductionStatusRepository _productionStatusRepo;
        private readonly ProductRepository _productRepo;

        public OrderController(
            OrderRepository orderRepo,
            OrderItemRepository orderItemRepo,
            CustomerRepository customerRepo,
            PersonnelRepository personnelRepo,
            ProductionStatusRepository productionStatusRepo,
            ProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _customerRepo = customerRepo;
            _personnelRepo = personnelRepo;
            _productionStatusRepo = productionStatusRepo;
            _productRepo = productRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowOrders)]
        public ActionResult AllOrders()
        {
            OrderListViewModel viewModel = new OrderListViewModel
            {
                OrdersPage = OrderPage.All,
                Orders = _orderRepo.Select().OrderByDescending(i => i.Id),
                Marketers = _personnelRepo.Where(i => i.JobTitleId == 0),
                Customers = _customerRepo.Select()
            };

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowOrders)]
        public ActionResult VerifyingOrders()
        {
            OrderListViewModel viewModel = new OrderListViewModel
            {
                OrdersPage = OrderPage.Verifying,
                Orders = _orderRepo.Where(i => !i.IsVerified && !i.FactorId.HasValue).OrderByDescending(i => i.Id),
                Marketers = _personnelRepo.Where(i => i.JobTitleId == 0),
                Customers = _customerRepo.Select()
            };

            return View("AllOrders", viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowOrders)]
        public ActionResult FactoringOrders()
        {
            OrderListViewModel viewModel = new OrderListViewModel
            {
                OrdersPage = OrderPage.Factoring,
                Orders = _orderRepo.Where(i => i.IsVerified && !i.FactorId.HasValue).OrderByDescending(i => i.Id),
                Marketers = _personnelRepo.Where(i => i.JobTitleId == 0),
                Customers = _customerRepo.Select()
            };

            return View("AllOrders", viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowOrders)]
        public ActionResult OrderDetail(int id)
        {
            OrderDetailsViewModel viewModel = new OrderDetailsViewModel
            {
                Order = _orderRepo.Find(id),
                Items = _orderItemRepo.Where(i => i.OrderId == id),
                ProductionStatuses = _productionStatusRepo.Select()
            };

            List<short> productIds = new List<short>();
            foreach (var orderItem in viewModel.Items)
            {
                productIds.Add(orderItem.ProductId);
            }

            var pendingCounts = _productRepo.GetLivePendingCount(productIds, true);

            foreach (var orderItem in viewModel.Items)
            {
                orderItem.Product.PendingsCount = pendingCounts[orderItem.ProductId];
            }

            return PartialView("_OrderDetail", viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.VerifyUnverifyOrder)]
        public JsonResult Verify(int id, Dictionary<int, double> items)
        {
            Order order = _orderRepo.Find(id);

            if (order == null)
            {
                return Json(new { Success = false });
            }

            _orderItemRepo.UpdateMainUnitCountRange(items);

            if (!_orderItemRepo.ItemsAreValid(order.OrderItems, out string message))
            {
                return Json(new { Success = false, Message = message });
            }

            order.IsVerified = true;

            if (_orderRepo.Update(order))
            {
                return Json(new { Success = true });
            }
            else
            {
                return Json(new { Success = false });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.VerifyUnverifyOrder)]
        public JsonResult Unverify(int id)
        {
            Order order = _orderRepo.Find(id);

            if (order == null)
            {
                return Json(new JsonData { Success = false });
            }

            order.IsVerified = false;

            if (_orderRepo.Update(order))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteOrder)]
        public JsonResult DeleteOrder(short id)
        {
            if (_orderRepo.Delete(id, out bool isUsed))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false, IsUsed = isUsed });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteOrder)]
        public JsonResult DeleteItem(int id)
        {
            if (_orderItemRepo.Delete(id, out bool isUsed))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false, IsUsed = isUsed });
            }
        }
    }
}