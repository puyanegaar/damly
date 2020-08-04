using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Warehouse;
using System.Web.Mvc;
using PunasMarketing.Models.Enums;

namespace PunasMarketing.Controllers
{
    public class WareHouseController : Controller
    {
        private readonly WarehouseRepository _blWarehouse;
        private readonly PersonnelRepository _blPersonnel;
        private readonly JobTitleRepository _blJobTitleRepository;

        public WareHouseController(
            WarehouseRepository blWarehouse,
            PersonnelRepository blPersonnelRepository,
            JobTitleRepository blJobTitleRepository)
        {
            _blWarehouse = blWarehouse;
            _blPersonnel = blPersonnelRepository;
            _blJobTitleRepository = blJobTitleRepository;
        }


        [HttpGet]
        [AccessControl(ActionsEnum.ShowWarehouses)]
        public ActionResult Warehouses()
        {
            return View(_blWarehouse.Select());
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddWarehouse)]
        public ActionResult AddWarehouse()
        {
            WarehouseViewModel warehouseViewModel = new WarehouseViewModel
            {
                Personnels = _blPersonnel.Select(),
                JobTitles = _blJobTitleRepository.Select()
            };
            return View(warehouseViewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.AddWarehouse)]
        public ActionResult AddWarehouse(WarehouseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (_blWarehouse.Add(viewModel.Warehouse))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ثبت شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    ModelState.Clear();
                    return RedirectToAction("Warehouses");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ثبت اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);
            }

            viewModel.Personnels = _blPersonnel.Select();
            viewModel.JobTitles = _blJobTitleRepository.Select();

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateWarehouse)]
        public ActionResult UpdateWarehouse(int id)
        {
            Warehouse warehouse = _blWarehouse.Find(id);
            if (warehouse == null)
            {
                return View("_Error404");
            }

            WarehouseViewModel warehouseViewModel = new WarehouseViewModel
            {
                Warehouse = warehouse,
                Personnels = _blPersonnel.Select(),
                JobTitles = _blJobTitleRepository.Select(),
                JobTitleId = warehouse.User.Personnel.JobTitleId
            };

            return View(warehouseViewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.UpdateWarehouse)]
        public ActionResult UpdateWarehouse(WarehouseViewModel warehouseViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_blWarehouse.Update(warehouseViewModel.Warehouse))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    ModelState.Clear();
                    return RedirectToAction("Warehouses");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);
            }

            return View(warehouseViewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteWarehouse)]
        public JsonResult DeleteWarehouse(int id)
        {
            if (_blWarehouse.Delete(id, out bool isUsed))
            {
                return Json(new JsonData() { Success = true });
            }
            else
            {
                return Json(new JsonData() { Success = false, IsUsed = isUsed });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetPersonnels(int? id)
        {
            if (id.HasValue)
            {
                WarehouseViewModel warehouseViewModel = new WarehouseViewModel
                {
                    Personnels = _blPersonnel.Where(i => i.User != null && i.JobTitleId == id.Value)
                };

                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_Personnels", warehouseViewModel)
                });
            }
            else
            {
                return null;
            }
        }
    }
}