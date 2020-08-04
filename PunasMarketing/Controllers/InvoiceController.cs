using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Invoice;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly PersonnelRepository _blPersonnel;
        private readonly WarehouseRepository _blWarehouse;
        private readonly InvoiceRepository _blInvoice;
        private readonly SectionRepository _blSection;
        private readonly ProductRepository _blProduct;
        private readonly InvoiceItemRepository _blInvoiceItem;
        private readonly FactorRepository _blFactor;

        public InvoiceController(PersonnelRepository blPersonnel, WarehouseRepository blWarehouse, InvoiceRepository blInvoice, SectionRepository blSection, ProductRepository blProduct, InvoiceItemRepository blInvoiceItem, FactorRepository blFactor)
        {
            _blPersonnel = blPersonnel;
            _blWarehouse = blWarehouse;
            _blInvoice = blInvoice;
            _blSection = blSection;
            _blProduct = blProduct;
            _blInvoiceItem = blInvoiceItem;
            _blFactor = blFactor;
        }


        #region Send Invoice
        [HttpGet]
        [AccessControl(ActionsEnum.ShowInvoices)]
        public ActionResult SendInvoices()
        {
            return View(_blInvoice.Where(a => !a.IsReceive).OrderByDescending(a => a.CreatedDateTime));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddInvoice)]
        public ActionResult AddSendInvoice()
        {
            var model = new InvoiceViewModel()
            {
                Personnels = _blPersonnel.Where(a => a.JobTitleId != 0),
                Warehouses = _blWarehouse.Select(),
                Sections = _blSection.Where(a => a.Id != 0),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddInvoice)]
        public ActionResult AddSendInvoice(Invoice invoiceModel, int invoicetype)
        {
            var model = new InvoiceViewModel()
            {
                InvoiceModel = invoiceModel,
                Personnels = _blPersonnel.Where(a => a.JobTitleId != 0),
                Warehouses = _blWarehouse.Select(),
                Sections = _blSection.Where(a => a.Id != 0),
            };

            if (ModelState.IsValid)
            {
                if (invoicetype == 0)
                {
                    TempData["SaveMessage"] = Notification.Show("نوع حواله باید انتخاب شود", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return View(model);
                }
                if (invoiceModel.OtherWareHouseId == null && invoiceModel.FactorNum == null && invoiceModel.PersonnelId == null)
                {
                    TempData["SaveMessage"] = Notification.Show("مقصد حواله باید مشخص شود", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return View(model);
                }

                invoiceModel.CreatedDateTime = DateTime.Now;
                invoiceModel.IsReceive = false;
                invoiceModel.IsCompleted = false;
                invoiceModel.CreatorUserId = UserIdentity.Getuserid();

                int invoiceId = _blInvoice.Add(invoiceModel);
                TempData["SaveMessage"] = Notification.Show("حواله جدید با موفقیت ایجاد شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                return Redirect("/Invoice/AddInvoiceItems/" + invoiceId);
            }

            TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
            return View(model);

        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateInvoice)]
        [InvoicesUpdateCheck]
        public ActionResult UpdateSendInvoice(int id)
        {
            var invoice = _blInvoice.Find(id);
            if (invoice.IsCompleted == true)
            {
                return RedirectToAction("SendInvoices");
            }
            if (invoice.PersonnelId != null)
            {
                invoice.SectionId = invoice.Personnel.JobTitle.SectionId;
            }
            var model = new InvoiceViewModel()
            {
                Personnels = _blPersonnel.Where(a => a.JobTitleId != 0),
                Warehouses = _blWarehouse.Select(),
                Sections = _blSection.Where(a => a.Id != 0),
                InvoiceModel = invoice

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateInvoice)]
        //[InvoicesUpdateCheck]
        public ActionResult UpdateSendInvoice(Invoice invoiceModel)
        {
            if (ModelState.IsValid)
            {
                if (_blInvoice.Update(invoiceModel))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return Redirect("/Invoice/AddInvoiceItems/" + invoiceModel.Id);
                }

            }
            var invoice = _blInvoice.Find(invoiceModel.Id);

            if (invoice.PersonnelId != null)
            {
                invoice.SectionId = invoice.Personnel.JobTitle.SectionId;
            }
            var model = new InvoiceViewModel()
            {
                Personnels = _blPersonnel.Where(a => a.JobTitleId != 0),
                Warehouses = _blWarehouse.Select(),
                Sections = _blSection.Where(a => a.Id != 0),
                InvoiceModel = invoice

            };

            TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
            return View(model);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddInvoice)]
        public ActionResult AddInvoiceItems(int id)
        {
            var invoice = _blInvoice.Find(id);
            var viewModel = new InvoiceItemViewModel
            {
                Invoice = invoice,
                Products = _blProduct.Where(i => i.WarehouseId == invoice.ThisWareHouseId),
                InvoiceItemModel = new InvoiceItem()
                {
                    InvoiceId = id,
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddInvoice)]
        public ActionResult AddInvoiceItems(InvoiceItem invoiceItemModel)
        {
            var dbInvocie = _blInvoice.Find(invoiceItemModel.InvoiceId);
            var model = new InvoiceItemViewModel()
            {
                Invoice = dbInvocie,
                Products = _blProduct.Where(i => i.WarehouseId == dbInvocie.ThisWareHouseId),
                InvoiceItemModel = invoiceItemModel

            };

            var invoice = _blInvoice.Find(invoiceItemModel.InvoiceId);

            if (ModelState.IsValid)
            {
                if (!invoice.IsReceive)
                {
                    if (!_blProduct.CheckInventory(invoiceItemModel.ProductId, invoiceItemModel.MainUnitCount))
                    {
                        TempData["SaveMessage"] = Notification.Show("تعداد انتخابی بیشتر از موجودی در دسترس است", type: ToastType.Error, position: ToastPosition.TopCenter);
                        return View(model);
                    }
                }

                if (_blInvoiceItem.Add(invoiceItemModel))
                {
                    _blInvoice.Save();
                    return View("InvoiceItemList", _blInvoiceItem.GetInvoiceItems(invoiceItemModel.InvoiceId));
                }

            }

            TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
            return View(model);

        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddInvoice)]
        public ActionResult SetComplete(int id)
        {
            var invoice = _blInvoice.Find(id);
            if (_blInvoice.SetComplete(id))
            {
                if (invoice.IsReceive)
                {
                    return RedirectToAction("ReceiveInvoices");
                }
                return RedirectToAction("SendInvoices");
            }

            return Redirect("/Invoice/AddSendInvoiceItems/" + id);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowInvoices)]
        public ActionResult InvoiceItemList(int id)
        {
            return PartialView(_blInvoiceItem.GetInvoiceItems(id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateInvoice)]
        public ActionResult EditInvoiceItem(int id)
        {
            var model = new InvoiceItemViewModel()
            {
                InvoiceItemModel = _blInvoiceItem.Find(id),
                Products = _blProduct.Select()
            };
            return PartialView(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateInvoice)]
        public ActionResult EditInvoiceItem(InvoiceItem invoiceItemModel)
        {

            var model = new InvoiceItemViewModel()
            {
                Invoice = _blInvoice.Find(invoiceItemModel.InvoiceId),
                Products = _blProduct.Select(),
                InvoiceItemModel = invoiceItemModel

            };

            if (ModelState.IsValid)
            {

                if (!_blProduct.CheckInventory(invoiceItemModel.ProductId, invoiceItemModel.MainUnitCount))
                {
                    ModelState.AddModelError("CustomError", "The item is removed from your cart");
                    return PartialView("EditInvoiceItem", model);
                }

                if (_blInvoiceItem.Update(invoiceItemModel))
                {
                    return View("InvoiceItemList", _blInvoiceItem.GetInvoiceItems(invoiceItemModel.InvoiceId));
                }

            }

            return View("InvoiceItemList", _blInvoiceItem.GetInvoiceItems(invoiceItemModel.InvoiceId));
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public JsonResult GetPersonnels(int? id)
        {
            if (id.HasValue)
            {
                InvoiceViewModel viewModel = new InvoiceViewModel();
                viewModel.Personnels = _blPersonnel.Where(i => i.JobTitle.SectionId == id.Value);


                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_InvoicePersonnel", viewModel),
                    Success = true
                });
            }
            else
            {
                InvoiceViewModel viewModel = new InvoiceViewModel();
                viewModel.Personnels = _blPersonnel.Where(i => i.JobTitle.SectionId == id.Value);
                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_InvoicePersonnel", viewModel)
                });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public JsonResult GetProducts(string prefix)
        {
            var products = _blProduct.Where(a => a.Name.StartsWith(prefix)).Select(a => new
            {
                label = a.Name,
                val = a.Id
            }).ToList();

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public double GetProductInventory(short id)
        {
            double inventory = _blProduct.GetProductInventory(id);

            return inventory;
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public JsonResult CheckInventory(short id, double inventory)
        {
            if (_blProduct.CheckInventory(id, inventory))
            {
                return Json(new JsonData() { Success = true });
            }
            else
            {
                return Json(new JsonData() { Success = false });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteInvoice)]
        [InvoicesUpdateCheck]
        public JsonResult DeleteInvoiceItem(int id)
        {
            if (_blInvoiceItem.Delete(id, out bool isUsed))
            {
                return Json(new JsonData() { Success = true });
            }
            else
            {
                return Json(new JsonData() { Success = false, IsUsed = isUsed });
            }
        }

        #endregion

        #region Receive
        [HttpGet]
        [AccessControl(ActionsEnum.ShowInvoices)]
        public ActionResult ReceiveInvoices()
        {
            return View(_blInvoice.Where(a => a.IsReceive).OrderByDescending(a => a.CreatedDateTime));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddInvoice)]
        public ActionResult AddReceiveInvoice()
        {
            var model = new InvoiceViewModel()
            {
                Personnels = _blPersonnel.Where(a => a.JobTitleId != 0),
                Warehouses = _blWarehouse.Select(),
                Sections = _blSection.Where(a => a.Id != 0),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddInvoice)]
        public ActionResult AddReceiveInvoice(Invoice invoiceModel, int invoicetype)
        {
            var model = new InvoiceViewModel()
            {
                InvoiceModel = invoiceModel,
                Personnels = _blPersonnel.Where(a => a.JobTitleId != 0),
                Warehouses = _blWarehouse.Select(),
                Sections = _blSection.Where(a => a.Id != 0),
            };

            if (ModelState.IsValid)
            {
                if (invoicetype == 0)
                {
                    TempData["SaveMessage"] = Notification.Show("نوع رسید باید انتخاب شود", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return View(model);
                }
                if (invoiceModel.OtherWareHouseId == null && invoiceModel.FactorNum == null && invoiceModel.PersonnelId == null)
                {
                    TempData["SaveMessage"] = Notification.Show("مبدا رسید باید مشخص شود", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return View(model);
                }
                invoiceModel.CreatedDateTime = DateTime.Now;
                invoiceModel.IsReceive = true;
                invoiceModel.IsCompleted = false;
                invoiceModel.CreatorUserId = UserIdentity.Getuserid();

                int invoiceId = _blInvoice.Add(invoiceModel);
                TempData["SaveMessage"] = Notification.Show("رسید جدید با موفقیت ایجاد شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                return Redirect("/Invoice/AddInvoiceItems/" + invoiceId);
            }

            TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
            return View(model);

        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateInvoice)]
        [InvoicesUpdateCheck]
        public ActionResult UpdateReceiveInvoice(int id)
        {
            var invoice = _blInvoice.Find(id);
            if (invoice.IsCompleted == true)
            {
                return RedirectToAction("ReceiveInvoices");
            }
            if (invoice.PersonnelId != null)
            {
                invoice.SectionId = invoice.Personnel.JobTitle.SectionId;
            }
            var model = new InvoiceViewModel()
            {
                Personnels = _blPersonnel.Where(a => a.JobTitleId != 0),
                Warehouses = _blWarehouse.Select(),
                Sections = _blSection.Where(a => a.Id != 0),
                InvoiceModel = invoice

            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateInvoice)]
        //[InvoicesUpdateCheck]
        public ActionResult UpdateReceiveInvoice(Invoice invoiceModel)
        {
            if (ModelState.IsValid)
            {
                if (_blInvoice.Update(invoiceModel))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return Redirect("/Invoice/AddInvoiceItems/" + invoiceModel.Id);
                }

            }
            var invoice = _blInvoice.Find(invoiceModel.Id);

            if (invoice.PersonnelId != null)
            {
                invoice.SectionId = invoice.Personnel.JobTitle.SectionId;
            }
            var model = new InvoiceViewModel()
            {
                Personnels = _blPersonnel.Where(a => a.JobTitleId != 0),
                Warehouses = _blWarehouse.Select(),
                Sections = _blSection.Where(a => a.Id != 0),
                InvoiceModel = invoice

            };

            TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
            return View(model);
        }

        #endregion

        [HttpGet]
        [AjaxOnly]
        [AccessControl(ActionsEnum.ShowInvoices)]
        public ActionResult InvoiceDetailsModal(int id)
        {
            Invoice invoice = _blInvoice.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            string residHavale = invoice.IsReceive ? "رسید" : "حواله";

            var thisWareHouseId = invoice.ThisWareHouseId;

            InvoiceDetailsViewModel viewModel = new InvoiceDetailsViewModel
            {
                Invoice = invoice,
                InvoiceItems = _blInvoiceItem.Where(i => i.InvoiceId == id).ToList()
            };

            return PartialView("_InvoiceDetails", viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteInvoice)]
        [InvoicesUpdateCheck]
        public JsonResult DeleteInvoice(int id)
        {
            if (_blInvoice.Delete(id, out bool isUsed))
            {
                return Json(new JsonData() { Success = true });
            }
            else
            {
                return Json(new JsonData() { Success = false, IsUsed = isUsed });
            }
        }
    }
}
