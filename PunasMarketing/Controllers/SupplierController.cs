using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Supplier;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class SupplierController : Controller
    {
        private readonly SupplierRepository _supplierRepo;
        private readonly ProvinceRepository _provinceRepo;
        private readonly CityRepository _cityRepo;
        private readonly TafsiliRepository _tafsiliRepo;

        public SupplierController(
            SupplierRepository supplierRepo,
            ProvinceRepository provinceRepo,
            CityRepository cityRepo,
            TafsiliRepository tafsiliRepo)
        {
            _supplierRepo = supplierRepo;
            _provinceRepo = provinceRepo;
            _cityRepo = cityRepo;
            _tafsiliRepo = tafsiliRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowSuppliers)]
        public ActionResult Suppliers()
        {
            return View(_supplierRepo.Select());
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddSupplier)]
        public ActionResult AddSupplier()
        {
            SupplierViewModel viewModel = new SupplierViewModel
            {
                Provinces = _provinceRepo.Select(),
                Supplier = new Supplier()
            };
            return View(viewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.AddSupplier)]
        public ActionResult AddSupplier(SupplierViewModel supplierViewModel)
        {
            if (ModelState.IsValid)
            {
                Supplier supplier = supplierViewModel.Supplier;

                if (_supplierRepo.Add(supplier))
                {
                    short addedSupplierId = _supplierRepo.GetMaxid();

                    List<Tafsili> tafsilis = new List<Tafsili>
                    {
                        new Tafsili
                        {
                            SupplierId = addedSupplierId,
                            PersonTypeId = 3,
                            SarfaslId = (short)SarfaslEnums.HesabDaryaftani,
                            TafsiliName = supplierViewModel.Supplier.Name
                        },
                        new Tafsili
                        {
                            SupplierId = addedSupplierId,
                            PersonTypeId = 3,
                            SarfaslId = (short)SarfaslEnums.AsnadDaryftani,
                            TafsiliName = supplierViewModel.Supplier.Name
                        },
                        new Tafsili
                        {
                            SupplierId = addedSupplierId,
                            PersonTypeId = 3,
                            SarfaslId = (short)SarfaslEnums.HesabPardakhtani,
                            TafsiliName = supplierViewModel.Supplier.Name
                        }
                    };

                    if (_tafsiliRepo.AddRange(tafsilis))
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        return RedirectToAction("Suppliers");
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات حسابهای تفصیلی", type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات تأمین کننده", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            SupplierViewModel viewModel = new SupplierViewModel
            {
                Supplier = supplierViewModel.Supplier,
                Provinces = _provinceRepo.Select()
            };

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateSupplier)]
        public ActionResult UpdateSupplier(int id)
        {
            var dbSupplier = _supplierRepo.Find(id);
            if (dbSupplier == null)
            {
                return View("_Error404");
            }

            SupplierViewModel viewModel = new SupplierViewModel
            {
                Supplier = dbSupplier,
                Provinces = _provinceRepo.Select(),
                Cities = _cityRepo.Where(i => i.ProvinceId == dbSupplier.City.ProvinceId)
            };
            viewModel.Supplier.ProvinceId = dbSupplier.City.ProvinceId;

            return View(viewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.UpdateSupplier)]
        public ActionResult UpdateSupplier(SupplierViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Supplier supplier = viewModel.Supplier;

                if (_supplierRepo.Update(supplier))
                {
                    if (_supplierRepo.UpdateTafsili(supplier.Id, supplier.Name))
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        return RedirectToAction("Suppliers");
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حساب تفصیلی", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.SupplierManagement)]
        public ActionResult SupplierManagement(int? id)
        {
            if (!id.HasValue)
            {
                return View("_Error404");
            }

            Supplier supplier = _supplierRepo.Find(id.Value);

            if (supplier == null)
            {
                return View("_Error404");
            }

            SupplierManagementViewModel viewModel = new SupplierManagementViewModel
            {
                Supplier = supplier,
                FinancialReport = _supplierRepo.GetFinancialReport(Fiscal.GetFiscalId(), id.Value),
            };

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetCities(int? id)
        {
            if (id.HasValue)
            {
                SupplierViewModel viewModel = new SupplierViewModel
                {
                    Cities = _cityRepo.Where(i => i.ProvinceId == id.Value)
                };

                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_Cities", viewModel)
                });
            }
            else
            {
                SupplierViewModel viewModel = new SupplierViewModel
                {
                    Cities = _cityRepo.Where(i => i.ProvinceId == id.Value)
                };

                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_Cities", viewModel)
                });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteSupplier)]
        public JsonResult DeleteSupplier(int id)
        {
            if (_supplierRepo.Delete(id, out bool isUsed))
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