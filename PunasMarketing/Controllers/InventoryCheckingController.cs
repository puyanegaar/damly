using Newtonsoft.Json;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.LocalModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.InventoryChecking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PunasMarketing.Models.Enums;

namespace PunasMarketing.Controllers
{
    public class InventoryCheckingController : Controller
    {
        private readonly InventoryCheckingRepository _inventoryCheckingRepo;
        private readonly InventoryCheckingItemRepository _inventoryCheckingItemRepo;
        private readonly WarehouseRepository _warehouseRepo;
        private readonly ProductRepository _productRepo;
        private readonly InvoiceRepository _invoiceRepo;
        private readonly InvoiceItemRepository _invoiceItemRepo;

        public InventoryCheckingController(
            InventoryCheckingRepository inventoryCheckingRepo,
            InventoryCheckingItemRepository inventoryCheckingItemRepo,
            WarehouseRepository warehouseRepo,
            ProductRepository productRepo,
            InvoiceRepository invoiceRepo,
            InvoiceItemRepository invoiceItemRepo)
        {
            _inventoryCheckingRepo = inventoryCheckingRepo;
            _inventoryCheckingItemRepo = inventoryCheckingItemRepo;
            _warehouseRepo = warehouseRepo;
            _productRepo = productRepo;
            _invoiceRepo = invoiceRepo;
            _invoiceItemRepo = invoiceItemRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowInventoryCheckings)]
        public ActionResult InventoryCheckings()
        {
            InventoryCheckingListViewModel viewModel = new InventoryCheckingListViewModel
            {
                InventoryCheckings = _inventoryCheckingRepo.Select().OrderByDescending(i => i.Id),
                Warehouses = _warehouseRepo.Select(),
                Items = _inventoryCheckingItemRepo.Select()
            };

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddInventoryChecking)]
        public ActionResult AddInventoryChecking()
        {
            InventoryCheckingViewModel viewModel = new InventoryCheckingViewModel
            {
                InventoryChecking = new InventoryChecking(),
                Warehouses = _warehouseRepo.Select(),
                SelectProducts = new List<Product>().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }),
                StrCreatedDate = DateTime.Now.ToPersianDateTime().ToStringDate()
            };

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.AddInventoryChecking)]
        public ActionResult AddInventoryCheckingAjax(AddUpdateInventoryCheckingAjaxViewModel viewModel)
        {
            viewModel.InventoryChecking.CreatedDate = viewModel.StrCreatedDate.ToMiladiDate();

            if (ModelState.IsValid)
            {
                viewModel.InventoryChecking.CreatedByUserId = UserIdentity.Getuserid(); //TODO: Logged in User
                if (_inventoryCheckingRepo.Add(viewModel.InventoryChecking))
                {
                    short inventoryCheckingId = _inventoryCheckingRepo.GetLastId();

                    if (_inventoryCheckingItemRepo.AddRange(viewModel.InventoryCheckingItems, inventoryCheckingId))
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);

                        return Json(new JsonData { Success = true });
                    }
                }
            }
            return Json(new JsonData { Success = false });
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateInventoryChecking)]
        [InventoryUpdateChecking]
        public ActionResult UpdateInventoryChecking(short id)
        {
            InventoryChecking inventoryChecking = _inventoryCheckingRepo.Find(id);
            if (inventoryChecking == null)
            {
                return View("_Error404");
            }

            InventoryCheckingViewModel viewModel = GetInvetoryCheckingVm(id);

            int index = 1;
            foreach (var item in viewModel.JsInventoryCheckingItemses)
            {
                item.Index = index;
                index++;
            }

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.UpdateInventoryChecking)]
        //[InventoryUpdateChecking]
        public ActionResult UpdateInventoryCheckingAjax(AddUpdateInventoryCheckingAjaxViewModel viewModel)
        {
            viewModel.InventoryChecking.CreatedDate = viewModel.StrCreatedDate.ToMiladiDate();

            if (ModelState.IsValid)
            {
                if (_inventoryCheckingRepo.Update(viewModel.InventoryChecking))
                {
                    _inventoryCheckingItemRepo.DeleteRange(viewModel.InventoryChecking.Id, false);
                    if (_inventoryCheckingItemRepo.AddRange(viewModel.InventoryCheckingItems, viewModel.InventoryChecking.Id))
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);

                        return Json(new JsonData { Success = true });
                    }
                }
            }
            return Json(new JsonData { Success = false });
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowInventoryCheckings)]
        public ActionResult Details(short id)
        {
            InventoryCheckingViewModel viewModel = GetInvetoryCheckingVm(id);

            int index = 1;
            foreach (var item in viewModel.JsInventoryCheckingItemses)
            {
                item.Index = index;
                index++;
            }

            return View(viewModel);
        }

        private InventoryCheckingViewModel GetInvetoryCheckingVm(short id)
        {
            InventoryCheckingViewModel viewModel = new InventoryCheckingViewModel
            {
                InventoryChecking = _inventoryCheckingRepo.Find(id),
                Warehouses = _warehouseRepo.Select(),
                SelectProducts = _productRepo.Select().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }),
                JsInventoryCheckingItemses = _inventoryCheckingItemRepo
                    .Where(i => i.InventoryCheckingId == id)
                    .Select(i => new JsInventoryCheckingItems
                    {
                        Index = 0,
                        InventoryCheckingItemId = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        MainUnit = i.Product.Unit.Name,
                        RealMainUnitCount = i.RealMainUnitCount,
                        SystemMainUnitCount = i.SystemMainUnitCount,
                        Difference = i.Difference,
                        IsCorrected = i.IsCorrected,
                        Description = i.Description ?? ""
                    })
            };
            viewModel.StrCreatedDate = viewModel.InventoryChecking.CreatedDate.ToPersianDateTime().ToStringDate();

            return viewModel;
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetProducts(int? id)
        {
            if (id.HasValue)
            {
                InventoryCheckingViewModel viewModel = new InventoryCheckingViewModel
                {
                    Products = _productRepo.Where(i => i.WarehouseId == id)
                };

                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_ProductDropDown", viewModel)
                });
            }
            else
            {
                InventoryCheckingViewModel viewModel = new InventoryCheckingViewModel
                {
                    Products = new List<Product>().AsQueryable()
                };

                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_ProductDropDown", viewModel)
                });
            }
        }

        [HttpPost]
        [AjaxOnly]
        [AccessControl(ActionsEnum.AddTadilInvoice)]
        public ActionResult TadilModal(TadilViewModel tadilViewModel)
        {
            tadilViewModel.InvoiceItemsJson = JsonConvert.SerializeObject(tadilViewModel.InvoiceItems);

            return Json(new JsonData
            {
                Success = true,
                Html = this.RenderPartialToString("_Tadil", tadilViewModel)
            });
        }

        [HttpPost]
        [AccessControl(ActionsEnum.AddTadilInvoice)]
        public ActionResult CreateTadilInvoiceModal(TadilViewModel tadilViewModel)
        {
            if (ModelState.IsValid)
            {
                tadilViewModel.Invoice.OtherWareHouseId = tadilViewModel.Invoice.ThisWareHouseId;
                tadilViewModel.Invoice.CreatedDateTime = DateTime.Now;
                tadilViewModel.Invoice.CreatorUserId = UserIdentity.Getuserid();
                tadilViewModel.Invoice.IsCompleted = true; // Has item(s)
                tadilViewModel.InvoiceItems = JsonConvert.DeserializeObject<IEnumerable<InvoiceItem>>(tadilViewModel.InvoiceItemsJson);

                int addedInvoiceId = _invoiceRepo.Add(tadilViewModel.Invoice);
                var invoiceItems = tadilViewModel.InvoiceItems.ToList();
                foreach (var invoiceItem in invoiceItems)
                {
                    invoiceItem.InvoiceId = addedInvoiceId;
                }

                if (_invoiceItemRepo.AddRange(invoiceItems))
                {
                    InventoryCheckingItem inventoryCheckingItem =
                        _inventoryCheckingItemRepo.Find(tadilViewModel.InventoryCheckingItemId);
                    inventoryCheckingItem.IsCorrected = true;

                    if (_inventoryCheckingItemRepo.Update(inventoryCheckingItem) &&
                        _invoiceRepo.SetComplete(addedInvoiceId) // Update product inventory
                        )
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    }
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ثبت اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
            }

            InventoryCheckingViewModel viewModel = GetInvetoryCheckingVm(tadilViewModel.InventoryCheckingId);
            return View("Details", viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteInventoryChecking)]
        [InventoryUpdateChecking]
        public JsonResult DeleteInventoryChecking(short id)
        {
            if (_inventoryCheckingRepo.Delete(id, out bool isUsed))
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