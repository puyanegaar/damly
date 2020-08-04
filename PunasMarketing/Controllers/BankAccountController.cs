using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.BankAccount;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class BankAccountController : Controller
    {

        private readonly BankAccountRepository _blBankAccount;
        private readonly BankRepository _blBank;
        private readonly TafsiliRepository _tafsiliRepo;

        public BankAccountController(
            BankAccountRepository blBankAccount,
            BankRepository blBank,
            TafsiliRepository tafsiliRepo)
        {
            _blBankAccount = blBankAccount;
            _blBank = blBank;
            _tafsiliRepo = tafsiliRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowBankAccounts)]
        public ActionResult BankAccounts()
        {
            return View(_blBankAccount.Select().OrderByDescending(i => i.Id));
        }


        [HttpGet]
        [AccessControl(ActionsEnum.AddBankAccount)]
        public ActionResult AddBankAccount()
        {
            BankAccountViewModel model = new BankAccountViewModel
            {
                Banks = _blBank.Select()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddBankAccount)]
        public ActionResult AddBankAccount(BankAccount BankAccountModel)
        {
            if (ModelState.IsValid)
            {
                if (_blBankAccount.Add(BankAccountModel))
                {
                    short addedBankAccId = _blBankAccount.GetMaxId();
                    string bankName = _blBank.Where(i => i.Id == BankAccountModel.BankId).FirstOrDefault()?.Name;
                    string tafsiliName = bankName + "-" + BankAccountModel.AccountNum;

                    List<Tafsili> tafsilis = new List<Tafsili>
                    {
                        new Tafsili
                        {
                            BankAccId = addedBankAccId,
                            SarfaslId = (short)SarfaslEnums.Bank,
                            TafsiliName = tafsiliName
                        },
                        new Tafsili
                        {
                            BankAccId = addedBankAccId,
                            SarfaslId = (short)SarfaslEnums.ChequeHayeDarJaryaneVosool,
                            TafsiliName = tafsiliName
                        },
                        new Tafsili
                        {
                            BankAccId = addedBankAccId,
                            SarfaslId = (short)SarfaslEnums.AsnadPardakhati,
                            TafsiliName = tafsiliName
                        }
                    };

                    if (_tafsiliRepo.AddRange(tafsilis))
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        ModelState.Clear();
                        return RedirectToAction("BankAccounts");
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات حسابهای تفصیلی", type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات حساب بانکی", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);
            }

            BankAccountViewModel model = new BankAccountViewModel { Banks = _blBank.Select() };
            return View(model);
        }


        [HttpGet]
        [AccessControl(ActionsEnum.UpdateBankAccount)]
        public ActionResult UpdateBankAccount(short id)
        {
            var bankAccount = _blBankAccount.Find(id);
            if (bankAccount == null)
            {
                return View("_Error404");
            }
            BankAccountViewModel model = new BankAccountViewModel
            {
                Banks = _blBank.Select(),
                BankAccountModel = bankAccount
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateBankAccount)]
        public ActionResult UpdateBankAccount(BankAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                BankAccount bankAccount = viewModel.BankAccountModel;
                if (_blBankAccount.Update(bankAccount))
                {
                    string bankName = _blBank.Where(i => i.Id == bankAccount.BankId).FirstOrDefault()?.Name;
                    string tafsiliName = bankName + "-" + bankAccount.AccountNum;
                    if (_blBankAccount.UpdateTafsili(bankAccount.Id, tafsiliName))
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        ModelState.Clear();
                        return RedirectToAction("BankAccounts");
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حسابهای تفصیلی", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حساب بانکی", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowBankAccounts)]
        public ActionResult BankAccountDetails(short id)
        {

            var bankAccount = _blBankAccount.Find(id);
            if (bankAccount == null)
            {
                return View("_Error404");
            }

            return View(bankAccount);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteBankAccount)]
        public JsonResult DeleteBankAccount(short id)
        {
            if (_blBankAccount.Delete(id, out bool isUsed))
            {
                return Json(new JsonData() { Success = true });
            }
            else
            {
                return Json(new JsonData() { Success = false, IsUsed = isUsed});
            }
        }

    }
}
