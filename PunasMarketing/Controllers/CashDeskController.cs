using System.Linq;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class CashDeskController : Controller
    {
        private readonly CashDeskRepository _blCashDesk;
        private readonly TafsiliRepository _tafsiliRepo;

        public CashDeskController(
            CashDeskRepository blCashDesk,
            TafsiliRepository tafsiliRepo)
        {
            _blCashDesk = blCashDesk;
            _tafsiliRepo = tafsiliRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowCashDesks)]
        public ActionResult CashDesks()
        {
            return View(_blCashDesk.Select().OrderByDescending(i => i.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddCashDesk)]
        public ActionResult AddCashDesk()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddCashDesk)]
        public ActionResult AddCashDesk(CashDesk cashDesk)
        {
            if (ModelState.IsValid)
            {
                if (_blCashDesk.Add(cashDesk))
                {
                    short addedCashDeskId = _blCashDesk.GetMaxId();

                    Tafsili tafsili = new Tafsili
                    {
                        CashDeskId = addedCashDeskId,
                        SarfaslId = (short)SarfaslEnums.CashDesk,
                        TafsiliName = cashDesk.Name
                    };

                    if (_tafsiliRepo.Add(tafsili))
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        ModelState.Clear();
                        return RedirectToAction("CashDesks");
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
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

            return View(cashDesk);
        }


        [HttpGet]
        [AccessControl(ActionsEnum.UpdateCashDesk)]
        public ActionResult UpdateCashDesk(short id)
        {
            CashDesk cashDesk = _blCashDesk.Find(id);
            if (cashDesk == null)
            {
                return View("_Error404");
            }
            return View(cashDesk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateCashDesk)]
        public ActionResult UpdateCashDesk(CashDesk cashDesk)
        {
            if (ModelState.IsValid)
            {
                if (_blCashDesk.Update(cashDesk))
                {
                    if (_blCashDesk.UpdateTafsili(cashDesk.Id, cashDesk.Name))
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        return RedirectToAction("CashDesks");
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حسابهای تفصیلی", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات صندوق", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);

            }

            return View(cashDesk);

        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteCashDesk)]
        public JsonResult DeleteCashDesk(short id)
        {
            if (_blCashDesk.Delete(id, out bool isUsed))
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
