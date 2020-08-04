using Microsoft.Ajax.Utilities;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.EntityModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Cheque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class ChequeController : Controller
    {
        private readonly ChequeRepository _chequeRepo;
        private readonly BankRepository _bankRepo;
        private readonly BankAccountRepository _bankAccountRepo;
        private readonly ChequeStatusRepository _chequeStatusRepo;
        private readonly ReceiptItemRepository _receiptItemRepo;
        private readonly CashDeskRepository _cashDeskRepo;
        private readonly SanadRepository _sanadRepo;
        private readonly SanadItemRepository _sanadItemRepo;
        private readonly TafsiliRepository _tafsiliRepo;

        public ChequeController(
            ChequeRepository chequeRepo,
            BankRepository bankRepo,
            BankAccountRepository bankAccountRepo,
            ChequeStatusRepository chequeStatusRepo,
            ReceiptItemRepository receiptItemRepo,
            CashDeskRepository cashDeskRepo,
            SanadRepository sanadRepo,
            SanadItemRepository sanadItemRepo,
            TafsiliRepository tafsiliRepo)
        {
            _chequeRepo = chequeRepo;
            _bankRepo = bankRepo;
            _bankAccountRepo = bankAccountRepo;
            _chequeStatusRepo = chequeStatusRepo;
            _receiptItemRepo = receiptItemRepo;
            _cashDeskRepo = cashDeskRepo;
            _sanadRepo = sanadRepo;
            _sanadItemRepo = sanadItemRepo;
            _tafsiliRepo = tafsiliRepo;
        }

        #region RecevieCheques

        [HttpGet]
        [AccessControl(ActionsEnum.ShowCheques)]
        public ActionResult RecieveCheques() // نمایش فهرست چک های دریافتی
        {
            var viewModel = new ChequeListViewModel
            {
                Cheques = _chequeRepo.Where(i => i.StatusId <= 10).OrderByDescending(i => i.Id), // دریافتی
                Banks = _bankRepo.Select(),
                ChequeStatuses = _chequeStatusRepo.Select()
            };
            return View(viewModel);
        }

        //////////////////////////////////////////////////////////////////

        [HttpGet]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ReceivedModal(int? id) // دریافتی
        {
            if (!id.HasValue)
            {
                return PartialView("_Error404");
            }

            Cheque cheque = _chequeRepo.Find(id.Value);

            if (cheque == null)
            {
                return PartialView("_Error404");
            }

            if (cheque.StatusId == (byte)ChequeStatus.ChequeKharj)
            {
                return PartialView("_WarningMessage", "این چک قبلا خرج شده است.");
            }

            return PartialView("_Received", id);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ReceivedModal(int id) // دریافتی
        {
            if (ModelState.IsValid)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                short fiscalId = Fiscal.GetFiscalId();
                Cheque cheque = _chequeRepo.Find(id);

                if (cheque.StatusId == (byte)ChequeStatus.ChequeKharj)
                {
                    return PartialView("_WarningMessage", "این چک قبلا خرج شده است.");
                }

                var sanadIds = _sanadItemRepo.Where(i => i.ChequeId == id).AsEnumerable()
                    .DistinctBy(i => i.SandId).Select(i => i.SandId).ToArray();

                List<Sanad> sanadsToRemove = new List<Sanad>();
                foreach (var sanadId in sanadIds)
                {
                    Sanad sanad = _sanadRepo.Find(sanadId, fiscalId);
                    if (sanad.SanadTypeId != (byte)SandType.Receive)
                    {
                        sanadsToRemove.Add(sanad);
                    }
                }

                if (sanadsToRemove.Any())
                {
                    bool sanadsAreSuccessfullyDeleted = _sanadRepo.DeleteRange(sanadsToRemove);
                    if (sanadsAreSuccessfullyDeleted == false)
                    {
                        return RedirectToAction("ChequeDetails", new { id = id });
                    }
                }

                cheque.StatusId = (byte)ChequeStatus.chequeDaryafti; // دریافتی
                cheque.Price = "foo";
                cheque.DudateText = "foo";

                bool chequeIsUpdatedSuccessfully = _chequeRepo.Update(cheque);
                if (chequeIsUpdatedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = id });
                }

                TempData["SaveMessage"] = Notification.Show("وضعیت چک به «دریافتی» تغییر کرد.", type: ToastType.Success, position: ToastPosition.TopCenter);
                ModelState.Clear();
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return RedirectToAction("ChequeDetails", new { id = id });
        }

        //////////////////////////////////////////////////////////////////

        [HttpGet]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult DepositReceiveModal(int id) // به حساب خواباندن چک دریافتی
        {
            Cheque cheque = _chequeRepo.Find(id);
            if (cheque.StatusId == (byte)ChequeStatus.ChequeKhabidebeHesab)
            {
                return PartialView("_WarningMessage", "این چک در حال حاضر در وضعیت خوابیده به حساب است.");
            }

            if (cheque.StatusId == (byte)ChequeStatus.ChequeVosolShode)
            {
                return PartialView("_WarningMessage", "این چک قبلا وصول شده است.");
            }

            if (cheque.StatusId == (byte)ChequeStatus.ChequeOdadt)
            {
                return PartialView("_WarningMessage", "این چک قبلا عودت داده شده است.");
            }

            if (cheque.StatusId == (byte)ChequeStatus.ChequeKharj)
            {
                return PartialView("_WarningMessage", "این چک قبلا خرج شده است.");
            }

            DepositReceiveChequeViewModel viewModel = new DepositReceiveChequeViewModel
            {
                ChequeId = id,
                BankAccounts = _bankAccountRepo.Select(),
                DepositDate = DateTime.Now.ToPersianDateTime().ToStringDate()
            };

            return PartialView("_DepositReceiveCheque", viewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult DepositReceiveModal(DepositReceiveChequeViewModel viewModel) // به حساب خواباندن چک دریافتی
        {
            if (ModelState.IsValid)
            {
                // حالت پیش فرض یعنی اگر به خطایی برخورد کرد
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);

                int chequeId = viewModel.ChequeId;
                Cheque cheque = _chequeRepo.Find(chequeId);
                string description = $"خواباندن چک شماره {cheque.ChequeNum} در حساب بانکی";

                cheque.Price = "foo";
                cheque.DudateText = "foo";
                cheque.StatusId = (byte)ChequeStatus.ChequeKhabidebeHesab; // خواباندن به حساب

                short fiscalId = Fiscal.GetFiscalId();
                int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                Sanad sanad = new Sanad
                {
                    Id = addedSanadId,
                    PeriodId = fiscalId,
                    Description = description,
                    IsConfirmed = true,
                    IsAutomatic = true,
                    SanadTypeId = (byte)SandType.Khabandan,
                    CreatedByUserId = 1, // TODO
                    ConfirmDate = viewModel.DepositDate.ToMiladiDate(),
                    CreatedDate = DateTime.Now,
                    TotalAmount = cheque.Amount
                };

                bool sanadIsAddedSuccessfully = _sanadRepo.Add(sanad);
                if (sanadIsAddedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                List<SanadItem> sanadItems = new List<SanadItem>();

                SanadItem bestan = GetBestanAsnadDaryaftaniSanadItem(
                    fiscalId, cheque, addedSanadId, description);
                if (bestan == null)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                bestan.ConfirmDate = viewModel.DepositDate.ToMiladiDate();
                sanadItems.Add(bestan);

                SanadItem bedeh = GetBedehChequeHayeDarJaryaneVosoolSanadItem(
                    fiscalId, cheque, addedSanadId, viewModel.BankAccountId, description);
                if (bedeh == null)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                bedeh.ConfirmDate = viewModel.DepositDate.ToMiladiDate();
                sanadItems.Add(bedeh);

                bool sanadItemsAreAddedSuccessfully = _sanadItemRepo.AddRange(sanadItems);
                if (sanadItemsAreAddedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                bool chequeIsUpdatedSuccessfully = _chequeRepo.Update(cheque);
                if (chequeIsUpdatedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                TempData["SaveMessage"] = Notification.Show("وضعیت چک به «خوابیده به حساب» تغییر کرد.", type: ToastType.Success, position: ToastPosition.TopCenter);
                ModelState.Clear();
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return RedirectToAction("ChequeDetails", new { id = viewModel.ChequeId });
        }

        //////////////////////////////////////////////////////////////////

        [HttpGet]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ClearReceiveModal(int id) // وصول کردن چک دریافتی
        {
            var cheque = _chequeRepo.Find(id);
            if (cheque == null)
            {
                return View("_Error404");
            }

            if (cheque.StatusId == (byte)ChequeStatus.ChequeKhabidebeHesab) // اگر قبلا در وضعیت به حساب خوابیده بود
            {
                ClearReceiveDepositedChequeViewModel clearReceiveDepositedChequeViewModel = new ClearReceiveDepositedChequeViewModel
                {
                    ChequeId = id,
                    DepositDate = DateTime.Now.ToPersianDateTime().ToStringDate()
                };
                return PartialView("_ClearReceiveDepositedCheque", clearReceiveDepositedChequeViewModel);
            }

            if (cheque.StatusId == (byte)ChequeStatus.ChequeOdadt)
            {
                return PartialView("_WarningMessage", "این چک قبلا عودت داده شده است.");
            }

            if (cheque.StatusId == (byte)ChequeStatus.ChequeKharj)
            {
                return PartialView("_WarningMessage", "این چک قبلا خرج شده است.");
            }

            ClearReceiveChequeViewModel clearReceiveChequeViewModel = new ClearReceiveChequeViewModel
            {
                ChequeId = id,
                BankAccounts = _bankAccountRepo.Select(),
                CashDesks = _cashDeskRepo.Select(),
                ClearDate = DateTime.Now.ToPersianDateTime().ToStringDate()
            };

            return PartialView("_ClearReceiveCheque", clearReceiveChequeViewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ClearReceiveDepositedModal(ClearReceiveDepositedChequeViewModel viewModel) //وصول چک خوابیده به حساب
        {
            if (ModelState.IsValid)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);

                Cheque cheque = _chequeRepo.Find(viewModel.ChequeId);
                string description = $"{cheque.ChequeNum} وصول چک خوابیده به حساب به شماره";

                if (cheque.StatusId != (byte)ChequeStatus.ChequeKhabidebeHesab)
                {
                    TempData["SaveMessage"] = Notification.Show("وضعیت چک باید قبلا در حالت خوابیده به حساب باشد.", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                cheque.Price = "foo";
                cheque.DudateText = "foo";
                cheque.StatusId = (byte)ChequeStatus.ChequeVosolShode; // چک دریافتی وصول شده

                short fiscalId = Fiscal.GetFiscalId();
                int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                Sanad sanad = new Sanad
                {
                    Id = addedSanadId,
                    PeriodId = fiscalId,
                    Description = description,
                    IsConfirmed = true,
                    IsAutomatic = true,
                    SanadTypeId = (byte)SandType.Vosool,
                    CreatedByUserId = 1, // TODO
                    ConfirmDate = viewModel.DepositDate.ToMiladiDate(),
                    CreatedDate = DateTime.Now,
                    TotalAmount = cheque.Amount
                };

                bool sanadIsAddedSuccessfully = _sanadRepo.Add(sanad);
                if (sanadIsAddedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                List<SanadItem> sanadItems = new List<SanadItem>();
                var darJaryaneVosoolSanadItem = _sanadItemRepo.Where(i =>
                    i.ChequeId == cheque.Id &&
                    i.SarfaslId == (int)SarfaslEnums.ChequeHayeDarJaryaneVosool).FirstOrDefault();

                if (darJaryaneVosoolSanadItem == null)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                var bankAccId = darJaryaneVosoolSanadItem.Tafsili.BankAccId;
                var bedeh = GetBedehBankSanadItem(fiscalId, cheque, addedSanadId, bankAccId, description);
                if (bedeh == null)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                bedeh.ConfirmDate = viewModel.DepositDate.ToMiladiDate();
                sanadItems.Add(bedeh);

                var bestan = ReverseBedehBestan(darJaryaneVosoolSanadItem);
                bestan.SandId = addedSanadId;
                bestan.PeriodId = fiscalId;
                bestan.Description = description;
                bestan.ConfirmDate = viewModel.DepositDate.ToMiladiDate();

                sanadItems.Add(bestan);

                bool sanadItemsAreAddedSuccessfully = _sanadItemRepo.AddRange(sanadItems);
                if (sanadItemsAreAddedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                bool chequeIsUpdatedSuccessfully = _chequeRepo.Update(cheque);
                if (chequeIsUpdatedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                TempData["SaveMessage"] = Notification.Show("وضعیت چک به «وصول شده» تغییر کرد.", type: ToastType.Success, position: ToastPosition.TopCenter);
                ModelState.Clear();
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return RedirectToAction("ChequeDetails", new { id = viewModel.ChequeId });
        }

        [HttpPost]
        [AjaxOnly]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ClearReceiveModalAjax(ClearReceiveChequeViewModel viewModel) // وصول کردن چک دریافتی
        {
            if (ModelState.IsValid)
            {
                // حالت پیش فرض یعنی اگر به خطایی برخورد کرد
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);

                int chequeId = viewModel.ChequeId;
                Cheque cheque = _chequeRepo.Find(chequeId);
                string description = $"وصول چک دریافت شده به شماره {cheque.ChequeNum}";

                cheque.Price = "foo";
                cheque.DudateText = "foo";
                cheque.StatusId = (byte)ChequeStatus.ChequeVosolShode; // وصول کردن چک دریافتی

                short fiscalId = Fiscal.GetFiscalId();
                int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                Sanad sanad = new Sanad
                {
                    Id = addedSanadId,
                    PeriodId = fiscalId,
                    Description = description,
                    IsConfirmed = true,
                    IsAutomatic = true,
                    SanadTypeId = (byte)SandType.Vosool,
                    CreatedByUserId = 1, // TODO
                    ConfirmDate = viewModel.ClearDate.ToMiladiDate(), // تاریخ سند
                    CreatedDate = DateTime.Now, // تاریخ ذخیره در دیتابیس
                    TotalAmount = cheque.Amount
                };

                bool sanadIsAddedSuccessfully = _sanadRepo.Add(sanad);
                if (sanadIsAddedSuccessfully == false)
                {
                    return Json(new JsonData { Success = false });
                }

                List<SanadItem> sanadItems = new List<SanadItem>();

                // سند بستانکاری در هر دو حالت واریز به حساب بانکی و واریز به صندوق یکسان بوده و اسناد دریافتنی فرد را بستانکار می کند
                SanadItem bestan = GetBestanAsnadDaryaftaniSanadItem(fiscalId, cheque, addedSanadId, description);
                if (bestan == null)
                {
                    return Json(new JsonData { Success = false });
                }

                bestan.ConfirmDate = viewModel.ClearDate.ToMiladiDate();
                sanadItems.Add(bestan);

                if (viewModel.BankAccountId.HasValue) // واریز به بانک
                {
                    SanadItem bedeh = GetBedehBankSanadItem(
                        fiscalId, cheque, addedSanadId, viewModel.BankAccountId, description);
                    if (bedeh == null)
                    {
                        return Json(new JsonData { Success = false });
                    }

                    bedeh.ConfirmDate = viewModel.ClearDate.ToMiladiDate();
                    sanadItems.Add(bedeh);
                }
                else if (viewModel.CashDeskId.HasValue) // واریز نقدی به صندوق
                {
                    SanadItem bedeh = GetBedehCashDeskSanadItem(
                        fiscalId, cheque, addedSanadId, viewModel.CashDeskId, description);
                    if (bedeh == null)
                    {
                        return Json(new JsonData { Success = false });
                    }

                    bedeh.ConfirmDate = viewModel.ClearDate.ToMiladiDate();
                    sanadItems.Add(bedeh);
                }

                bool sanadItemsAreAddedSuccessfully = _sanadItemRepo.AddRange(sanadItems);
                if (sanadItemsAreAddedSuccessfully == false)
                {
                    return Json(new JsonData { Success = false });
                }

                bool chequeIsUpdatedSuccessfully = _chequeRepo.Update(cheque);
                if (chequeIsUpdatedSuccessfully == false)
                {
                    return Json(new JsonData { Success = false });
                }

                TempData["SaveMessage"] = Notification.Show("وضعیت چک به «وصول شده» تغییر کرد.", type: ToastType.Success, position: ToastPosition.TopCenter);
                ModelState.Clear();

                return Json(new JsonData { Success = true });
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return Json(new JsonData { Success = false });
        }

        //////////////////////////////////////////////////////////////////

        [HttpGet]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ReturnedReceiveModal(int id) // عودت دادن چک دریافتی
        {
            Cheque cheque = _chequeRepo.Find(id);

            if (cheque.StatusId != (byte)ChequeStatus.chequeDaryafti &&
                cheque.StatusId != (byte)ChequeStatus.ChequeKharj)
            {
                return PartialView("_WarningMessage", "برای عودت چک، باید وضعیت آن در حالت «دریافتی» باشد.");
            }

            ReturnReceiveChequeViewModel viewModel = new ReturnReceiveChequeViewModel
            {
                ChequeId = id,
                ReturnDate = DateTime.Now.ToPersianDateTime().ToStringDate()
            };

            return PartialView("_ReturnedReceiveCheque", viewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ReturnedReceiveModal(ReturnReceiveChequeViewModel viewModel) // عودت دادن چک دریافتی
        {
            if (ModelState.IsValid)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);

                Cheque cheque = _chequeRepo.Find(viewModel.ChequeId);
                if (cheque.StatusId != (byte)ChequeStatus.chequeDaryafti &&
                    cheque.StatusId != (byte)ChequeStatus.ChequeKharj)
                {
                    TempData["SaveMessage"] = Notification.Show("برای عودت چک، باید وضعیت آن در حالت «دریافتی» باشد.", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                if (cheque.StatusId == (byte)ChequeStatus.chequeDaryafti)
                {
                    string description = $"عودت چک دریافتی به شماره {cheque.ChequeNum}";

                    var chequeSanadItems = _sanadItemRepo.Where(i => i.ChequeId == cheque.Id);
                    SanadItem receiveBedeh = chequeSanadItems.FirstOrDefault(i => i.Bedeh > 0 && i.Bestan == 0);
                    SanadItem receiveBestan = chequeSanadItems.FirstOrDefault(i => i.Bedeh == 0 && i.Bestan > 0);

                    if (chequeSanadItems.Count() != 2 || receiveBedeh == null || receiveBestan == null
                        || receiveBestan.Bestan != cheque.Amount || receiveBedeh.Bedeh != cheque.Amount)
                    {
                        TempData["SaveMessage"] = Notification.Show("سند حسابداری دریافت چک به درستی ثبت نشده است.", type: ToastType.Error, position: ToastPosition.TopCenter);
                        return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                    }

                    short fiscalId = Fiscal.GetFiscalId();
                    int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                    Sanad sanad = new Sanad
                    {
                        Id = addedSanadId,
                        PeriodId = fiscalId,
                        Description = description,
                        IsConfirmed = true,
                        IsAutomatic = true,
                        SanadTypeId = (byte)SandType.Odat,
                        CreatedByUserId = 1, // TODO LOGIN
                        ConfirmDate = viewModel.ReturnDate.ToMiladiDate(),
                        CreatedDate = DateTime.Now,
                        TotalAmount = cheque.Amount
                    };

                    bool sanadIsAddedSuccessfully = _sanadRepo.Add(sanad);
                    if (sanadIsAddedSuccessfully == false)
                    {
                        return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                    }

                    List<SanadItem> sanadItemsToAdd = new List<SanadItem>();

                    SanadItem bedeh = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        ChequeId = cheque.Id,
                        SarfaslId = receiveBestan.SarfaslId,
                        TafsiliId = receiveBestan.TafsiliId,
                        Description = description,
                        Bestan = 0,
                        Bedeh = receiveBestan.Bestan,
                        ConfirmDate = viewModel.ReturnDate.ToMiladiDate()
                    };
                    sanadItemsToAdd.Add(bedeh);

                    SanadItem bestan = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        ChequeId = cheque.Id,
                        SarfaslId = receiveBedeh.SarfaslId,
                        TafsiliId = receiveBedeh.TafsiliId,
                        Description = description,
                        Bedeh = 0,
                        Bestan = receiveBedeh.Bedeh,
                        ConfirmDate = viewModel.ReturnDate.ToMiladiDate()
                    };
                    sanadItemsToAdd.Add(bestan);

                    bool sanadItemsAreAddedSuccessfully = _sanadItemRepo.AddRange(sanadItemsToAdd);
                    if (sanadItemsAreAddedSuccessfully == false)
                    {
                        return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                    }
                }
                else if (cheque.StatusId == (byte)ChequeStatus.ChequeKharj)
                {
                    string description = $"عودت چک خرج شده به شماره {cheque.ChequeNum}";

                    var chequeSanadItems = _sanadItemRepo.Where(i => i.ChequeId == cheque.Id);
                    var receiveBestan = chequeSanadItems.FirstOrDefault(i =>
                        i.SarfaslId == (short)SarfaslEnums.HesabDaryaftani && i.Bestan > 0 && i.Bedeh == 0);
                    var receiveBedeh = chequeSanadItems.FirstOrDefault(i =>
                        i.SandId == receiveBestan.SandId && i.Bedeh > 0 && i.Bestan == 0);

                    var kharjBestan = chequeSanadItems.FirstOrDefault(i =>
                        i.SandId != receiveBestan.SandId && i.Bestan > 0 && i.Bedeh == 0);
                    var kharjBedeh = chequeSanadItems.FirstOrDefault(i =>
                        i.SandId != receiveBedeh.SandId && i.Bedeh > 0 && i.Bestan == 0);

                    if (chequeSanadItems.Count() != 4 || receiveBedeh == null || receiveBestan == null || kharjBestan == null || kharjBedeh == null)
                    {
                        TempData["SaveMessage"] = Notification.Show("سند حسابداری دریافت و خرج چک به درستی ثبت نشده است.", type: ToastType.Error, position: ToastPosition.TopCenter);
                        return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                    }

                    short fiscalId = Fiscal.GetFiscalId();
                    int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                    Sanad sanad = new Sanad
                    {
                        Id = addedSanadId,
                        PeriodId = fiscalId,
                        Description = description,
                        IsConfirmed = true,
                        IsAutomatic = true,
                        SanadTypeId = (byte)SandType.Odat,
                        CreatedByUserId = 1, // TODO LOGIN
                        ConfirmDate = viewModel.ReturnDate.ToMiladiDate(),
                        CreatedDate = DateTime.Now,
                        TotalAmount = cheque.Amount
                    };

                    bool sanadIsAddedSuccessfully = _sanadRepo.Add(sanad);
                    if (sanadIsAddedSuccessfully == false)
                    {
                        return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                    }

                    List<SanadItem> sanadItemsToAdd = new List<SanadItem>();

                    SanadItem reverseReceiveBestan = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        ChequeId = cheque.Id,
                        SarfaslId = receiveBestan.SarfaslId,
                        TafsiliId = receiveBestan.TafsiliId,
                        Description = description,
                        Bestan = 0,
                        Bedeh = receiveBestan.Bestan,
                        ConfirmDate = viewModel.ReturnDate.ToMiladiDate()
                    };
                    sanadItemsToAdd.Add(reverseReceiveBestan);

                    SanadItem reverseReceiveBedeh = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        ChequeId = cheque.Id,
                        SarfaslId = receiveBedeh.SarfaslId,
                        TafsiliId = receiveBedeh.TafsiliId,
                        Description = description,
                        Bestan = receiveBedeh.Bedeh,
                        Bedeh = 0,
                        ConfirmDate = viewModel.ReturnDate.ToMiladiDate()
                    };
                    sanadItemsToAdd.Add(reverseReceiveBedeh);

                    SanadItem reverseKharjBestan = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        ChequeId = cheque.Id,
                        SarfaslId = kharjBestan.SarfaslId,
                        TafsiliId = kharjBestan.TafsiliId,
                        Description = description,
                        Bestan = 0,
                        Bedeh = kharjBestan.Bestan,
                        ConfirmDate = viewModel.ReturnDate.ToMiladiDate()
                    };
                    sanadItemsToAdd.Add(reverseKharjBestan);

                    SanadItem reverseKharjBedeh = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        ChequeId = cheque.Id,
                        SarfaslId = kharjBedeh.SarfaslId,
                        TafsiliId = kharjBedeh.TafsiliId,
                        Description = description,
                        Bestan = kharjBedeh.Bedeh,
                        Bedeh = 0,
                        ConfirmDate = viewModel.ReturnDate.ToMiladiDate()
                    };
                    sanadItemsToAdd.Add(reverseKharjBedeh);

                    bool sanadItemsAreAddedSuccessfully = _sanadItemRepo.AddRange(sanadItemsToAdd);
                    if (sanadItemsAreAddedSuccessfully == false)
                    {
                        return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                    }
                }

                cheque.Price = "foo";
                cheque.DudateText = "foo";
                cheque.StatusId = (byte)ChequeStatus.ChequeOdadt; // عودت شده

                bool chequeIsUpdated = _chequeRepo.Update(cheque);
                if (chequeIsUpdated == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                TempData["SaveMessage"] = Notification.Show("وضعیت چک به «عودت شده» تغییر کرد.", type: ToastType.Success, position: ToastPosition.TopCenter);
                ModelState.Clear();
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return RedirectToAction("ChequeDetails", new { id = viewModel.ChequeId });
        }

        #endregion


        #region PayCheques

        [HttpGet]
        [AccessControl(ActionsEnum.ShowCheques)]
        public ActionResult PayCheques()
        {
            var viewModel = new ChequeListViewModel
            {
                Cheques = _chequeRepo.Where(i => i.StatusId > 10).OrderByDescending(i => i.Id), // پرداختی
                Accounts = _bankAccountRepo.Select(),
                ChequeStatuses = _chequeStatusRepo.Select()
            };
            return View(viewModel);
        }

        //////////////////////////////////////////////////////////////////       

        [HttpGet]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult PayedModal(int? id) // پرداختی
        {
            if (!id.HasValue)
            {
                return PartialView("_Error404");
            }

            Cheque cheque = _chequeRepo.Find(id.Value);

            if (cheque == null)
            {
                return PartialView("_Error404");
            }

            return PartialView("_Payed", id);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult PayedModal(int id) // پرداختی
        {
            if (ModelState.IsValid)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                short fiscalId = Fiscal.GetFiscalId();
                Cheque cheque = _chequeRepo.Find(id);

                var sanadIds = _sanadItemRepo.Where(i => i.ChequeId == id).AsEnumerable()
                    .DistinctBy(i => i.SandId).Select(i => i.SandId);

                List<Sanad> sanadsToRemove = new List<Sanad>();
                foreach (var sanadId in sanadIds)
                {
                    Sanad sanad = _sanadRepo.Find(sanadId, fiscalId);
                    if (sanad.SanadTypeId != (byte)SandType.Payment)
                    {
                        sanadsToRemove.Add(sanad);
                    }
                }

                if (sanadsToRemove.Any())
                {
                    bool sanadsAreSuccessfullyDeleted = _sanadRepo.DeleteRange(sanadsToRemove);
                    if (sanadsAreSuccessfullyDeleted == false)
                    {
                        return RedirectToAction("ChequeDetails", new { id = id });
                    }
                }

                cheque.StatusId = (byte)ChequeStatus.ChequePardakhti; // پرداختی
                cheque.Price = "foo";
                cheque.DudateText = "foo";

                bool chequeIsUpdatedSuccessfully = _chequeRepo.Update(cheque);
                if (chequeIsUpdatedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = id });
                }

                TempData["SaveMessage"] = Notification.Show("وضعیت چک به «پرداختی» تغییر کرد.", type: ToastType.Success, position: ToastPosition.TopCenter);
                ModelState.Clear();
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return RedirectToAction("ChequeDetails", new { id = id });
        }

        //////////////////////////////////////////////////////////////////

        [HttpGet]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ClearPayModal(int id) // وصول چک پرداختی
        {
            Cheque cheque = _chequeRepo.Find(id);

            if (cheque.StatusId == (byte)ChequeStatus.ChequeOdatPardakhti)
            {
                return PartialView("_WarningMessage", "این چک قبلا عودت گرفته شده است.");
            }

            if (cheque.StatusId == (byte)ChequeStatus.ChequeVosolPardakhti)
            {
                return PartialView("_WarningMessage", "وضعیت چک باید قبلا در حالت پرداختی باشد.");
            }

            ClearPayChequeViewModel viewModel = new ClearPayChequeViewModel
            {
                ChequeId = id,
                ClearDate = DateTime.Now.ToPersianDateTime().ToStringDate()
            };
            return PartialView("_ClearPayCheque", viewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ClearPayModal(ClearPayChequeViewModel viewModel) // وصول چک پرداختی
        {
            if (ModelState.IsValid)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);

                Cheque cheque = _chequeRepo.Find(viewModel.ChequeId);
                string description = $"وصول چک پرداخت شده به شماره {cheque.ChequeNum}";

                if (cheque.StatusId != (byte)ChequeStatus.ChequePardakhti)
                {
                    TempData["SaveMessage"] = Notification.Show("وضعیت چک باید قبلا در حالت پرداختی باشد.", type: ToastType.Error, position: ToastPosition.TopCenter);
                }

                cheque.Price = "foo";
                cheque.DudateText = "foo";
                cheque.StatusId = (byte)ChequeStatus.ChequeVosolPardakhti; // چک پرداختی وصول شده

                var chequeSanadItems = _sanadItemRepo.Where(i => i.ChequeId == cheque.Id);
                var payBestan = chequeSanadItems.FirstOrDefault(i =>
                    i.SarfaslId == (short)SarfaslEnums.AsnadPardakhati && i.Bestan > 0);
                var payBedeh = chequeSanadItems.FirstOrDefault(i =>
                    i.SarfaslId != (short)SarfaslEnums.AsnadPardakhati && i.Bedeh > 0);

                if (chequeSanadItems.Count() != 2 || payBestan == null || payBedeh == null)
                {
                    TempData["SaveMessage"] = Notification.Show("سند حسابداری پرداخت چک به درستی ثبت نشده است.", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                short fiscalId = Fiscal.GetFiscalId();
                int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                Sanad sanad = new Sanad
                {
                    Id = addedSanadId,
                    PeriodId = fiscalId,
                    Description = description,
                    IsConfirmed = true,
                    IsAutomatic = true,
                    SanadTypeId = (byte)SandType.VosoolPardakhti,
                    CreatedByUserId = 1, // TODO
                    ConfirmDate = viewModel.ClearDate.ToMiladiDate(),
                    CreatedDate = DateTime.Now,
                    TotalAmount = cheque.Amount
                };

                bool sanadIsAddedSuccessfully = _sanadRepo.Add(sanad);
                if (sanadIsAddedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                List<SanadItem> sanadItems = new List<SanadItem>();

                SanadItem vosoolBedeh = new SanadItem
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    ChequeId = cheque.Id,
                    SarfaslId = (short)SarfaslEnums.AsnadPardakhati,
                    TafsiliId = payBestan.TafsiliId,
                    Description = description,
                    Bestan = 0,
                    Bedeh = cheque.Amount,
                    ConfirmDate = viewModel.ClearDate.ToMiladiDate()
                };
                sanadItems.Add(vosoolBedeh);

                if (payBestan.TafsiliId.HasValue == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }
                Tafsili payBestanTafsili = _tafsiliRepo.Find(payBestan.TafsiliId.Value);
                var bankAccId = payBestanTafsili.BankAccId;
                if (bankAccId == null)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                var bankTafsili = _tafsiliRepo.Where(i =>
                    i.BankAccId == bankAccId && i.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                if (bankTafsili == null)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                SanadItem vosoolBestan = new SanadItem
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    ChequeId = cheque.Id,
                    SarfaslId = (short)SarfaslEnums.Bank,
                    TafsiliId = bankTafsili.Id,
                    Description = description,
                    Bedeh = 0,
                    Bestan = cheque.Amount,
                    ConfirmDate = viewModel.ClearDate.ToMiladiDate()
                };
                sanadItems.Add(vosoolBestan);

                bool sanadItemsAreAddedSuccessfully = _sanadItemRepo.AddRange(sanadItems);
                if (sanadItemsAreAddedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                bool chequeIsUpdatedSuccessfully = _chequeRepo.Update(cheque);
                if (chequeIsUpdatedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                ModelState.Clear();
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return RedirectToAction("ChequeDetails", new { id = viewModel.ChequeId });
        }


        //////////////////////////////////////////////////////////////////

        [HttpGet]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ReturnedPayModal(int? id) // عودت گرفتن چک پرداختی
        {
            if (!id.HasValue)
            {
                return PartialView("_WarningMessage", "خطا در بارگزاری صفحه");
            }

            Cheque cheque = _chequeRepo.Find(id.Value);
            if (cheque == null)
            {
                return View("_Error404");
            }

            if (cheque.StatusId == (byte)ChequeStatus.ChequeVosolPardakhti)
            {
                return PartialView("_WarningMessage", "این چک قبلا وصول شده و امکان عودت گرفتن آن وجود ندارد. برای عودت چک باید وضعیت آن در حالت پرداختی باشد.");
            }

            ReturnPayChequeViewModel viewModel = new ReturnPayChequeViewModel
            {
                ChequeId = id.Value,
                ReturnDate = DateTime.Now.ToPersianDateTime().ToStringDate()
            };

            return PartialView("_ReturnedPayCheque", viewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.ChangeStatus)]
        public ActionResult ReturnedPayModal(ReturnPayChequeViewModel viewModel) // عودت گرفتن چک پرداختی
        {
            if (ModelState.IsValid)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);

                Cheque cheque = _chequeRepo.Find(viewModel.ChequeId);
                string description = $"عودت گرفتن چک پرداختی به شماره {cheque.ChequeNum}";

                if (cheque.StatusId == (byte)ChequeStatus.ChequeVosolPardakhti)
                {
                    TempData["SaveMessage"] = Notification.Show("این چک قبلا وصول شده است.", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                cheque.Price = "foo";
                cheque.DudateText = "foo";
                cheque.StatusId = (byte)ChequeStatus.ChequeOdatPardakhti; // عودت شده

                var chequeSanadItems = _sanadItemRepo.Where(i => i.ChequeId == cheque.Id);
                var payBestan = chequeSanadItems.FirstOrDefault(i => i.Bestan > 0 && i.Bedeh == 0);
                var payBedeh = chequeSanadItems.FirstOrDefault(i => i.Bedeh > 0 && i.Bestan == 0);

                if (chequeSanadItems.Count() != 2 || payBestan == null || payBedeh == null)
                {
                    TempData["SaveMessage"] = Notification.Show("سند حسابداری پرداخت چک به درستی ثبت نشده است.", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                short fiscalId = Fiscal.GetFiscalId();
                int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                Sanad sanad = new Sanad
                {
                    Id = addedSanadId,
                    PeriodId = fiscalId,
                    Description = description,
                    IsConfirmed = true,
                    IsAutomatic = true,
                    SanadTypeId = (byte)SandType.OdatPardakhti,
                    CreatedByUserId = 1, // TODO
                    ConfirmDate = viewModel.ReturnDate.ToMiladiDate(),
                    CreatedDate = DateTime.Now,
                    TotalAmount = cheque.Amount
                };

                bool sanadIsAddedSuccessfully = _sanadRepo.Add(sanad);
                if (sanadIsAddedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                List<SanadItem> sanadItems = new List<SanadItem>();

                SanadItem reversePayBedeh = new SanadItem
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    ChequeId = cheque.Id,
                    SarfaslId = payBedeh.SarfaslId,
                    TafsiliId = payBedeh.TafsiliId,
                    Description = description,
                    Bestan = payBedeh.Bedeh,
                    Bedeh = 0,
                    ConfirmDate = viewModel.ReturnDate.ToMiladiDate()
                };
                sanadItems.Add(reversePayBedeh);

                SanadItem reversePayBestan = new SanadItem
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    ChequeId = cheque.Id,
                    SarfaslId = payBestan.SarfaslId,
                    TafsiliId = payBestan.TafsiliId,
                    Description = description,
                    Bedeh = payBestan.Bestan,
                    Bestan = 0,
                    ConfirmDate = viewModel.ReturnDate.ToMiladiDate()
                };
                sanadItems.Add(reversePayBestan);

                bool sanadItemsAreAddedSuccessfully = _sanadItemRepo.AddRange(sanadItems);
                if (sanadItemsAreAddedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                bool chequeIsUpdatedSuccessfully = _chequeRepo.Update(cheque);
                if (chequeIsUpdatedSuccessfully == false)
                {
                    return RedirectToAction("ChequeDetails", new { id = cheque.Id });
                }

                TempData["SaveMessage"] = Notification.Show("وضعیت چک به «عودت شده» تغییر کرد.", type: ToastType.Success, position: ToastPosition.TopCenter);
                ModelState.Clear();
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return RedirectToAction("ChequeDetails", new { id = viewModel.ChequeId });
        }

        //////////////////////////////////////////////////////////////////

        #endregion

        [HttpGet]
        [AccessControl(ActionsEnum.ShowCheques)]
        public ActionResult ChequeDetails(int id)
        {
            Cheque theCheque = _chequeRepo.Find(id);
            theCheque.StatusName = _chequeStatusRepo.Find(theCheque.StatusId).Name;
            theCheque.CounterPartyName = _chequeRepo.GetChequeCounterParty(id).FirstOrDefault();
            ReceiptItem receiptItem = _receiptItemRepo.Where(i => i.ChequeId == id).FirstOrDefault();
            if (receiptItem != null)
            {
                theCheque.ReceiptId = receiptItem.ReceiptId;
            }

            List<ChequeStatusMetaData> chequeStatuses;
            if (theCheque.StatusId <= 10) // دریافتی
            {
                chequeStatuses = _chequeStatusRepo.GetReceiveStatuses().ToList();
            }
            else // پرداختی
            {
                chequeStatuses = _chequeStatusRepo.GetPayStatuses().ToList();
            }

            string chequeAccountNumber = string.Empty;
            if (theCheque.BankAccountId != null)
            {
                chequeAccountNumber = _bankAccountRepo.Find(theCheque.BankAccountId.Value).BankNameAcc;
            }

            ChequeDetailsViewModel viewModel = new ChequeDetailsViewModel
            {
                Cheque = theCheque,
                ChequeStatuses = chequeStatuses,
                ChequeAccountNum = chequeAccountNumber,
            };
            var chequeSanadItems = _sanadItemRepo.Where(i => i.ChequeId == theCheque.Id);
            viewModel.ChequeSanads = new List<Sanad>();
            foreach (var chequeSanadItem in chequeSanadItems)
            {
                if (!viewModel.ChequeSanads.Any(i =>
                    i.Id == chequeSanadItem.SandId && i.PeriodId == chequeSanadItem.PeriodId))
                {
                    viewModel.ChequeSanads.Add(_sanadRepo.Find(chequeSanadItem.SandId, chequeSanadItem.PeriodId));
                }
            }

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteCheque)]
        public JsonResult DeleteCheque(int id)
        {
            if (_chequeRepo.Delete(id, out bool isUsed))
            {
                TempData["SaveMessage"] = Notification.Show("چک مورد نظر با موفقیت حذف شد.", type: ToastType.Success, position: ToastPosition.TopCenter);
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false, IsUsed = isUsed });
            }
        }

        #region GetSanadItem

        /// <summary>
        /// جای بدهکار و بستانکار را در سند آیتم با همان مبلغ عوض می کند.
        /// مقدار فیلد آیدی سند و توضیحات باید به صورت دستی تنظیم شود.
        /// </summary>
        /// <returns></returns>
        private SanadItem ReverseBedehBestan(SanadItem sanadItem)
        {
            if (sanadItem == null)
            {
                return null;
            }

            SanadItem result = new SanadItem
            {
                ChequeId = sanadItem.ChequeId,
                SarfaslId = sanadItem.SarfaslId,
                TafsiliId = sanadItem.TafsiliId
            };

            if (sanadItem.Bedeh == 0)
            {
                result.Bedeh = sanadItem.Bestan;
                result.Bestan = 0;
            }
            else if (sanadItem.Bestan == 0)
            {
                result.Bestan = sanadItem.Bedeh;
                result.Bedeh = 0;
            }

            return result;
        }

        /// <summary>
        /// آیتم سند بدهکاری خواباندن چک به حساب بانکی.
        /// حساب چکهای در جریان وصول را بدهکار می کند.
        /// </summary>
        /// <returns></returns>
        private SanadItem GetBedehChequeHayeDarJaryaneVosoolSanadItem(
            short fiscalId, Cheque cheque, int addedSanadId, short? bankAccId, string description)
        {
            if (!bankAccId.HasValue)
            {
                return null;
            }

            var chequeHayeDarJaryaneVosoolTafsili = _tafsiliRepo.Where(i =>
                i.BankAccId == bankAccId &&
                i.SarfaslId == (int)SarfaslEnums.ChequeHayeDarJaryaneVosool).FirstOrDefault();

            if (chequeHayeDarJaryaneVosoolTafsili?.SarfaslId == null)
            {
                return null;
            }

            return new SanadItem
            {
                SandId = addedSanadId,
                PeriodId = fiscalId,
                ChequeId = cheque.Id,
                SarfaslId = (int)SarfaslEnums.ChequeHayeDarJaryaneVosool,
                TafsiliId = chequeHayeDarJaryaneVosoolTafsili.Id,
                Description = description,
                Bestan = 0,
                Bedeh = cheque.Amount
            };
        }

        /// <summary>
        /// آیتم سند بستانکاری وصول چک خوابیده به حساب.
        /// حساب چکهای در جریان وصول را بستانکار می کند.
        /// </summary>
        /// <returns></returns>
        private SanadItem GetBestanChequeHayeDarJaryaneVosoolSanadItem(
            short fiscalId, Cheque cheque, int addedSanadId, short? bankAccId, string description)
        {
            var bedeh = GetBedehChequeHayeDarJaryaneVosoolSanadItem(fiscalId, cheque, addedSanadId, bankAccId, description);
            bedeh.Bestan = bedeh.Bedeh;
            bedeh.Bedeh = 0;

            return bedeh;
        }

        /// <summary>
        /// آیتم سند حسابداری بستانکاری وصول چک به حساب بانکی.
        /// اسناد دریافتنی فردی را که چک را از او دریافت کرده ایم، بستانکار می کند.
        /// </summary>
        /// <returns></returns>
        private SanadItem GetBestanAsnadDaryaftaniSanadItem(
            short fiscalId, Cheque cheque, int addedSanadId, string description)
        {
            var chequeSanads = _sanadItemRepo.Where(i => i.ChequeId == cheque.Id);
            var asnadDaryaftaniSanad = chequeSanads.FirstOrDefault(
                i => i.SarfaslId == (int)SarfaslEnums.AsnadDaryftani);
            if (asnadDaryaftaniSanad == null)
            {
                return null;
            }

            return new SanadItem
            {
                SandId = addedSanadId,
                PeriodId = fiscalId,
                ChequeId = cheque.Id,
                SarfaslId = asnadDaryaftaniSanad.SarfaslId,
                TafsiliId = asnadDaryaftaniSanad.TafsiliId,
                Description = description,
                Bestan = cheque.Amount,
                Bedeh = 0
            };
        }

        /// <summary>
        /// آیتم سند حسابداری بدهکاری وصول چک به حساب بانکی.
        /// حساب بانکی را که مبلغ چک به آن واریز می شود، بدهکار می کند.
        /// </summary>
        /// <returns></returns>
        private SanadItem GetBedehBankSanadItem(
            short fiscalId, Cheque cheque, int addedSanadId, short? bankAccId, string description)
        {
            if (!bankAccId.HasValue)
            {
                return null;
            }

            var bankTafsili = _tafsiliRepo.Where(i =>
                i.BankAccId == bankAccId &&
                i.SarfaslId == (int)SarfaslEnums.Bank).FirstOrDefault();

            if (bankTafsili?.SarfaslId == null)
            {
                return null;
            }

            return new SanadItem
            {
                SandId = addedSanadId,
                PeriodId = fiscalId,
                ChequeId = cheque.Id,
                SarfaslId = (int)SarfaslEnums.Bank,
                TafsiliId = bankTafsili.Id,
                Description = description,
                Bestan = 0,
                Bedeh = cheque.Amount
            };
        }

        /// <summary>
        /// آیتم سند حسابداری بدهکاری وصول نقدی چک به صندوق.
        /// صندوقی را که مبلغ چک نقدا به آن واریز می شود، بدهکار می کند.
        /// </summary>
        /// <returns></returns>
        private SanadItem GetBedehCashDeskSanadItem(
            short fiscalId, Cheque cheque, int addedSanadId, short? cashDeskId, string description)
        {
            if (!cashDeskId.HasValue)
            {
                return null;
            }

            var cashDeskTafsili = _tafsiliRepo.Where(i =>
                i.CashDeskId == cashDeskId &&
                i.SarfaslId == (int)SarfaslEnums.CashDesk).FirstOrDefault();

            if (cashDeskTafsili?.SarfaslId == null)
            {
                return null;
            }

            return new SanadItem
            {
                SandId = addedSanadId,
                PeriodId = fiscalId,
                ChequeId = cheque.Id,
                SarfaslId = (int)SarfaslEnums.CashDesk,
                TafsiliId = cashDeskTafsili.Id,
                Description = description,
                Bestan = 0,
                Bedeh = cheque.Amount
            };
        }

        private SanadItem GetBestanBankSanadItem(short fiscalId, Cheque cheque, int addedSanadId, short? bankAccId, string description)
        {
            SanadItem bestan = ReverseBedehBestan(GetBedehBankSanadItem(fiscalId, cheque, addedSanadId, bankAccId, description));
            bestan.SandId = addedSanadId;
            bestan.Description = description;

            return bestan;
        }

        private SanadItem GetBedehAsnadPardakhtaniSanadItem(short fiscalId, Cheque cheque, int addedSanadId, string description)
        {
            var chequeSanads = _sanadItemRepo.Where(i => i.ChequeId == cheque.Id);
            var asnadPardakhtaniSanad = chequeSanads.FirstOrDefault(
                i => i.SarfaslId == (int)SarfaslEnums.AsnadPardakhati);
            if (asnadPardakhtaniSanad == null)
            {
                return null;
            }

            return new SanadItem
            {
                SandId = addedSanadId,
                PeriodId = fiscalId,
                ChequeId = cheque.Id,
                SarfaslId = asnadPardakhtaniSanad.SarfaslId,
                TafsiliId = asnadPardakhtaniSanad.TafsiliId,
                Description = description,
                Bestan = 0,
                Bedeh = cheque.Amount
            };
        }

        private SanadItem GetBestanHesabPardakhtani(short fiscalId, Cheque cheque, int addedSanadId, string description)
        {
            var chequeSanads = _sanadItemRepo.Where(i => i.ChequeId == cheque.Id);
            var hesabPardakhtaniSanad = chequeSanads.FirstOrDefault(
                i => i.SarfaslId == (int)SarfaslEnums.HesabPardakhtani);
            if (hesabPardakhtaniSanad == null)
            {
                return null;
            }

            return new SanadItem
            {
                SandId = addedSanadId,
                PeriodId = fiscalId,
                ChequeId = cheque.Id,
                SarfaslId = hesabPardakhtaniSanad.SarfaslId,
                TafsiliId = hesabPardakhtaniSanad.TafsiliId,
                Description = description,
                Bestan = cheque.Amount,
                Bedeh = 0
            };
        }

        #endregion
    }
}