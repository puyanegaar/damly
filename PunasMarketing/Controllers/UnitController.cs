using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using System.Web.Mvc;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Models.Enums;

namespace PunasMarketing.Controllers
{
    public class UnitController : Controller
    {
        private readonly UnitRepository _blUnit;

        public UnitController(UnitRepository blUnit)
        {
            _blUnit = blUnit;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowUnits)]
        public ActionResult Units()
        {
            return View(_blUnit.Select());
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddUnit)]
        public ActionResult AddUnit()
        {
          return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddUnit)]
        public ActionResult AddUnit(Unit unit)
        {
            if (ModelState.IsValid)
            {
                if (_blUnit.Add(unit))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("Units");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);
             
            }

            return View(unit);

        }
        
        [HttpGet]
        [AccessControl(ActionsEnum.UpdateUnit)]
        public ActionResult UpdateUnit(int id)
        {
            var unit = _blUnit.Find(id);
            if (unit == null)
            {
                return View("_Error404");
            }
          return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateUnit)]
        public ActionResult UpdateUnit(Unit job)
        {
            if (ModelState.IsValid)
            {
                if (_blUnit.Update(job))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("Units");
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

            return View(job);

        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteUnit)]
        public JsonResult DeleteUnit(int id)
        {
            if (_blUnit.Delete(id, out bool isUsed))
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