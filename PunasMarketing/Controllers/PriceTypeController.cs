using System.Web.Mvc;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;

namespace PunasMarketing.Controllers
{
    public class PriceTypeController : Controller
    {
        private readonly PriceTypeRepository _blPriceType;

        public PriceTypeController(PriceTypeRepository blPriceType)
        {
            _blPriceType = blPriceType;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowPriceTypes)]
        public ActionResult PriceTypes()
        {
            return View(_blPriceType.Select());
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddPriceType)]
        public ActionResult AddPriceType()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddPriceType)]
        public ActionResult AddPriceType(PriceType priceType)
        {
            if (ModelState.IsValid)
            {
                if (_blPriceType.Add(priceType))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("PriceTypes");
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

            return View(priceType);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdatePriceType)]
        public ActionResult UpdatePriceType(int id)
        {
            var priceType = _blPriceType.Find(id);
            if (priceType == null)
            {
                return HttpNotFound();
            }
            return View(priceType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdatePriceType)]
        public ActionResult UpdatePriceType(PriceType priceType)
        {
            if (ModelState.IsValid)
            {
                if (_blPriceType.Update(priceType))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("PriceTypes");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);

            }
            return View(priceType);

        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeletePriceType)]
        public JsonResult DeletePriceType(int id)
        {
            if (_blPriceType.Delete(id, out bool isUsed))
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