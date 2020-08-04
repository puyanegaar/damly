using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PunasMarketing.Models.Enums;

namespace PunasMarketing.Controllers
{
    public class BankController : Controller
    {
        private readonly BankRepository _bankRepo;
        private readonly BankAccountRepository _bankAccRepo;
        private readonly TafsiliRepository _tafsiliRepo;

        public BankController(
            BankRepository bankRepo,
            BankAccountRepository bankAccRepo,
            TafsiliRepository tafsiliRepo)
        {
            _bankRepo = bankRepo;
            _bankAccRepo = bankAccRepo;
            _tafsiliRepo = tafsiliRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowBanks)]
        public ActionResult Banks()
        {
            return View(_bankRepo.Select().OrderByDescending(i => i.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddBank)]
        public ActionResult AddBank()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddBank)]
        public ActionResult AddBank(Bank bank)
        {
            if (ModelState.IsValid)
            {
                if (_bankRepo.Add(bank))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("Banks");
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

            return View(bank);
        }


        [HttpGet]
        [AccessControl(ActionsEnum.UpdateBank)]
        public ActionResult UpdateBank(short? id)
        {
            if (!id.HasValue)
            {
                return View("_Error404");
            }

            Bank bank = _bankRepo.Find(id.Value);
            if (bank == null)
            {
                return View("_Error404");
            }

            return View(bank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateBank)]
        public ActionResult UpdateBank(Bank bank)
        {
            if (ModelState.IsValid)
            {
                if (_bankRepo.Update(bank))
                {
                    List<BankAccount> bankAccs = _bankAccRepo.Where(i => i.BankId == bank.Id).ToList();
                    bool tafsiliUpdatedSuccessfully = true;
                    foreach (var bankAccount in bankAccs)
                    {
                        string bankName = bank.Name;
                        string tafsiliName = bankName + "-" + bankAccount.AccountNum;
                        if (!_bankAccRepo.UpdateTafsili(bankAccount.Id, tafsiliName))
                        {
                            tafsiliUpdatedSuccessfully = false;
                        }
                    }

                    if (tafsiliUpdatedSuccessfully)
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حساب های تفصیلی", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    }

                    return RedirectToAction("Banks");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات بانک", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);

            }

            return View(bank);

        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteBank)]
        public JsonResult DeleteBank(short id)
        {
            if (_bankRepo.Delete(id, out bool isUsed))
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