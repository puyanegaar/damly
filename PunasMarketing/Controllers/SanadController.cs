using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.LocalModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Sanad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class SanadController : Controller
    {
        private readonly PersonnelRepository _blPersonnel;
        private readonly BankAccountRepository _blBank;
        private readonly CustomerRepository _blcustomers;
        private readonly SupplierRepository _blsuppliers;
        private readonly CashDeskRepository _blCashDesk;
        private readonly BankRepository _blBankName;
        private readonly ChequeRepository _blcheque;
        private readonly ReceiptRepository _blReceipt;
        private readonly ReceiptItemRepository _blReceiptItem;
        private readonly TransactionRepository _blTransaction;
        private readonly SarfaslRepository _blSarfasl;
        private readonly TafsiliRepository _blTafsili;
        private readonly SanadRepository _blSanad;
        private readonly SanadItemRepository _blSanadItem;
        private readonly FiscalRepository _blFiscal;

        // public object Sand { get; private set; }

        public SanadController(
            PersonnelRepository PersonnelRepository,
            BankAccountRepository BankAccountRepostiory,
            CustomerRepository CustomerRepostiory,
            SupplierRepository SupplierRepository,
            CashDeskRepository cashdeskRepository,
            BankRepository bankRepository,
            ChequeRepository chequeRepository,
            ReceiptRepository receiptRepository,
            ReceiptItemRepository receiptItemRepository,
            TransactionRepository transactionRepository,
            SarfaslRepository sarfaslRepository,
            TafsiliRepository tafsiliRepository,
            SanadItemRepository sanadItemRepository,
            SanadRepository sanadRepository,
            FiscalRepository fiscalRepository
           )
        {
            _blPersonnel = PersonnelRepository;
            _blBank = BankAccountRepostiory;
            _blcustomers = CustomerRepostiory;
            _blsuppliers = SupplierRepository;
            _blCashDesk = cashdeskRepository;
            _blBankName = bankRepository;
            _blcheque = chequeRepository;
            _blReceipt = receiptRepository;
            _blReceiptItem = receiptItemRepository;
            _blTransaction = transactionRepository;
            _blSarfasl = sarfaslRepository;
            _blTafsili = tafsiliRepository;
            _blSanad = sanadRepository;
            _blSanadItem = sanadItemRepository;
            _blFiscal = fiscalRepository;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowAsnad)]
        public ActionResult Sanads()
        {
            short fiscalId = Fiscal.GetFiscalId();
            return View(_blSanad.Where(i => i.PeriodId == fiscalId).OrderByDescending(i => i.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowAsnadDasti)]
        public ActionResult AsnadDasti()
        {
            short fiscalId = Fiscal.GetFiscalId();
            return View(_blSanad.Where(i => i.SanadTypeId == (byte)SandType.SanadDasti && i.PeriodId == fiscalId)
                .OrderByDescending(i =>i.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateSanadDasti)]
        [FiscalCheck]
        [SanadUpdateCheck]
        public ActionResult UpdateSanad(int? SanadId)
        {
            if (SanadId.HasValue)
            {
                var model = new SanadViewModel();
                List<SanadItemModel> lst = new List<SanadItemModel>();
                short FiscalId = Fiscal.GetFiscalId();
                var sanad = _blSanad.Find(SanadId.Value, FiscalId);
                model.SanadId = SanadId.Value;
                model.SanadDes = sanad.Description;
                model.SanadDate = sanad.ConfirmDate.ToPersianDateTime().ToString("yyyy/MM/dd");
                var sanadItem = sanad.SanadItems;
                foreach (var item in sanadItem)
                {
                    var Sarfasl = item.Sarfasl;
                    string TafsiliName = "";
                    if (item.TafsiliId != null)
                    {
                        TafsiliName = _blTafsili.Where(m => m.Id == item.TafsiliId).Single().TafsiliName;
                    }

                    lst.Add(new SanadItemModel() { ItemId = item.Id, Sarfasl = item.SarfaslId, Tafsili = item.TafsiliId, Description = item.Description, BedehAmount = item.Bedeh, BestanAmount = item.Bestan, Code = Sarfasl.Coding, SarfaslName = Sarfasl.Name, TafsiliName = TafsiliName });
                    model.TotalBedeh += item.Bedeh;
                    model.TotalBestan += item.Bestan;
                    model.TotalDiff = Math.Abs(model.TotalBedeh - model.TotalBestan);
                }
                model.SanadItem = lst;
                model.SarFasl = _blSarfasl.Where(m => m.IsMoeen == true).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Coding + "-" + s.Name });
                return View(model);
            }
            else
            {
                return View();
            }

        }
        
        [HttpGet]
        [AccessControl(ActionsEnum.AddSanadDasti)]
        [FiscalCheck]
        public ActionResult AddSanad()
        {
            var model = new SanadViewModel();
            model.SarFasl = _blSarfasl.Where(m => m.IsMoeen == true).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Coding + "-" + s.Name });
            model.SanadDate = DateTime.Now.ToPersianDateTime().ToString("yyyy/MM/dd");
            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.AddSanadDasti)]
        [FiscalCheck]
        public ActionResult AddSanad(IEnumerable<SanadItemModel> SanadItem,string SanadDes,string SanadDate)
        {
           
            if (SanadItem != null && !string.IsNullOrEmpty(SanadDes) && !string.IsNullOrEmpty(SanadDate))
            {
                decimal Bedeh = SanadItem.Sum(m => m.BedehAmount);
                decimal Bestan = SanadItem.Sum(m => m.BestanAmount);
                if (Bedeh == Bestan)
                {
                    Sanad sanad = new Sanad();
                    sanad.CreatedDate = DateTime.Now;
                    sanad.ConfirmDate = SanadDate.ToMiladiDate();
                    sanad.SanadTypeId = (byte)SandType.SanadDasti;
                    sanad.IsAutomatic = false;
                    sanad.IsConfirmed = true;
                    sanad.CreatedByUserId = UserIdentity.Getuserid();
                    sanad.TotalAmount = Bestan;
                    sanad.Description = SanadDes;
                    short FiscalId = Fiscal.GetFiscalId();
                    sanad.PeriodId = FiscalId;
                    sanad.Id = _blSanad.GetLastIdentity(FiscalId)+1;
                    if (_blSanad.Add(sanad))
                    {
                        int sanadId = sanad.Id;
                        foreach (var item in SanadItem)
                        {
                            _blSanadItem.Add(sanadId, FiscalId,sanad.ConfirmDate, null, null, item.Sarfasl, item.Tafsili, item.Description, item.BedehAmount, item.BestanAmount, false);
                        }
                        if (_blSanadItem.Save())
                        {
                            TempData["saveMassage"] = Notification.Show("سند با موفقیت ثبت شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                            return Json(new JsonData
                            {
                                Url = Url.Action("AddSanad", "Sanad"),
                                Success = true
                            });
                        }
                        else
                        {
                            return Json(new JsonData
                            {
                                Script = Notification.BriefShow("ثبت سند با خطا مواجه شد", type: ToastType.Error),
                                Success = false
                            });
                        }
                    }
                    else
                    {

                        return Json(new JsonData
                        {
                            Script = Notification.BriefShow("ثبت سند با خطا مواجه شد", type: ToastType.Error),
                            Success = false
                        });
                    }
                }
                else
                {

                    return Json(new JsonData
                    {
                        Script = Notification.BriefShow("سند تراز نیست. جمع بدهکار و بستانکار برابر نمی باشد", type: ToastType.Warning),
                        Success = false
                    });
                }

            }
            else
            {

                return Json(new JsonData
                {
                    Script = Notification.BriefShow("اطلاعات سند را تکمیل کنید", type: ToastType.Warning),
                    Success = false
                });
            }

        }
        
        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.UpdateSanadDasti)]
        [FiscalCheck]
       
        public ActionResult UpdateSanad(IEnumerable<SanadItemModel> SanadItem, string SanadDes, string SanadDate, string SanadItemDeleted, int SanadId)
        {
            if (SanadItem != null && !string.IsNullOrEmpty(SanadDes) && !string.IsNullOrEmpty(SanadDate))
            {
                decimal Bedeh = SanadItem.Sum(m => m.BedehAmount);
                decimal Bestan = SanadItem.Sum(m => m.BestanAmount);
                short FiscalId = Fiscal.GetFiscalId();
                if (Bedeh == Bestan)
                {
                    Sanad sanad = _blSanad.Find(SanadId, FiscalId);
                    sanad.ConfirmDate = SanadDate.ToMiladiDate();
                    sanad.SanadTypeId = (byte)SandType.SanadDasti;
                    sanad.IsAutomatic = false;
                    sanad.IsConfirmed = true;
                    sanad.CreatedByUserId = 1;
                    sanad.TotalAmount = Bestan;
                    sanad.Description = SanadDes;
                    if (_blSanad.Update(sanad))
                    {
                        if (!string.IsNullOrEmpty(SanadItemDeleted))
                        {
                            string[] deleteItem = SanadItemDeleted.Split(',');
                            for (int i = 0; i < deleteItem.Length; i++)
                            {
                                int id = int.Parse(deleteItem[i]);
                                _blSanadItem.Delete(id, false);
                            }
                            _blSanadItem.Save();
                        }
                        foreach (var item in SanadItem)
                        {
                            if (item.ItemId == 0)
                            {
                                _blSanadItem.Add(SanadId, FiscalId, sanad.ConfirmDate, null, null, item.Sarfasl, item.Tafsili, item.Description, item.BedehAmount, item.BestanAmount, false);
                            }
                            else
                            {
                                _blSanadItem.Update(item.ItemId, SanadId, FiscalId,sanad.ConfirmDate, null, null, item.Sarfasl, item.Tafsili, item.Description, item.BedehAmount, item.BestanAmount, false);
                            }

                        }
                        if (_blSanadItem.Save())
                        {
                            TempData["saveMassage"] = Notification.Show("سند با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                            return Json(new JsonData
                            {
                                Url = Url.Action("UpdateSanad", "Sanad", new { SanadId = SanadId }),
                                Success = true
                            });
                        }
                        else
                        {
                            return Json(new JsonData
                            {
                                Script = Notification.BriefShow("ویرایش سند با خطا مواجه شد", type: ToastType.Error),
                                Success = false
                            });
                        }
                    }
                    else
                    {

                        return Json(new JsonData
                        {
                            Script = Notification.BriefShow("ویرایش سند با خطا مواجه شد", type: ToastType.Error),
                            Success = false
                        });
                    }
                }
                else
                {

                    return Json(new JsonData
                    {
                        Script = Notification.BriefShow("سند تراز نیست. جمع بدهکار و بستانکار برابر نمی باشد", type: ToastType.Warning),
                        Success = false
                    });
                }

            }
            else
            {

                return Json(new JsonData
                {
                    Script = Notification.BriefShow("اطلاعات سند را تکمیل کنید", type: ToastType.Warning),
                    Success = false
                });
            }

        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetTafsili(short SarfaslId)
        {
            var model = new SanadViewModel();
            model.Tafsili = _blTafsili.Where(m => m.SarfaslId == SarfaslId);
            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_DrpTafsili", model)
            });
        }

        [CoustomAuthrize]
        public ActionResult SanadDetail(int SanadId)
        {
            var model = new SanadViewModel
            {
                SanadDetail = _blSanad.GetSanadDetail(Fiscal.GetFiscalId(), SanadId),
                Sanad = _blSanad.Find(SanadId, Fiscal.GetFiscalId())
            };

            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_SanadDetail", model)
            });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteSanadDasti)]
        [FiscalCheck]
        [SanadUpdateCheck]
        public JsonResult DeleteSanad(int id)
        {
            short fiscalId = Fiscal.GetFiscalId();

            Sanad sanad = _blSanad.Find(id, fiscalId);
            if (sanad.SanadTypeId != (byte)SandType.SanadDasti)
            {
                return Json(new JsonData { Success = false });
            }

            if (_blSanad.Delete(sanad, out bool isUsed))
            {
                TempData["SaveMessage"] = Notification.Show("سند مورد نظر با موفقیت حذف شد.", type: ToastType.Success, position: ToastPosition.TopCenter);
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false, IsUsed = isUsed });
            }
        }
    }
}