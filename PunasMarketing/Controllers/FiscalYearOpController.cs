using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.FiscalYearOp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class FiscalYearOpController : Controller
    {
        #region Const
        private const string FiscalYearItem = "FiscalYearItem";
        private const string ReviewedItem = "ReviewedItem";
        private const string TavazonList = "TavazonList";
        private const string DbName = "DamliDB";

        #endregion
        #region Repository
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
        private readonly ReportsRepository _blReport;
        private readonly FiscalRepository _blFiscal;
        private readonly FiscalYearOpRepository _blFiscalYearOp;

        public FiscalYearOpController(
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
            ReportsRepository reportsRepository,
            FiscalRepository fiscalRepository,
            FiscalYearOpRepository fiscalYearOpRepository
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
            _blReport = reportsRepository;
            _blFiscal = fiscalRepository;
            _blFiscalYearOp = fiscalYearOpRepository;
        }
        #endregion

        #region CloseFinanceYear
        [HttpGet]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult CloseFinanceYear()
        {
            var model = new CloseFinanViewmodel();
            model.Fiscal = _blFiscal.Find(Fiscal.GetFiscalId());
            if (!model.Fiscal.IsClosed)
            {
                model.StartDate = model.Fiscal.EndDate.AddDays(1).ToPersianDateTime().ToString("yyyy/MM/dd");
                return View(model);
            }
            else
            {
                TempData["IndexMessage"] = Notification.Show("سال مالی انتخاب شده قبلا بسته شده است", type: ToastType.Warning, position: ToastPosition.TopCenter);
                return RedirectToAction("Index", "Home");
            }

        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult CloseFinanceYear(string startDate, string EndDate, short FiscalId, string FiscalName,byte Vat)
        {
            //first we should Take Backup
            var originalDirectory = new DirectoryInfo(string.Format("{0}App_Data\\Backups\\", AppDomain.CurrentDomain.BaseDirectory)).ToString();
            var date = DateTime.Now;
            string BackupName = DbName + "_" + date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString();
            int reslut = _blFiscalYearOp.BackUpDb(DbName, BackupName, originalDirectory);
            if (reslut == 1)
            {
                originalDirectory.ZipFile(BackupName);//make ZipFile from Backup
                short NewFiscalId = (short)(FiscalId + 1);
                int SanadId = _blSanad.GetLastIdentity(FiscalId);
                int TavazonSanadId = SanadId + 1;
                int CloseSanadId = TavazonSanadId + 1;
                int OpenSanadId = 1;
                var Fiscal = _blFiscalYearOp.GetFiscalObject(NewFiscalId, FiscalName, startDate.ToMiladiDate().GetStartDate(), EndDate.ToMiladiDate().GetEndDate(), date,Vat);
                var ItemList = (List<SanadItem>)Session[FiscalYearItem];
                var TavazonItem = (List<SanadItem>)Session[TavazonList];
                decimal CloseBed = ItemList.Where(m => m.PeriodId == FiscalId).Sum(m => m.Bedeh);
                decimal CloseBes = ItemList.Where(m => m.PeriodId == FiscalId).Sum(m => m.Bestan);
                decimal TavazonBed = TavazonItem.Sum(m => m.Bedeh);
                decimal TavazonBes = TavazonItem.Sum(m => m.Bestan);
                decimal TotalAmount = 0;
                if (CloseBed < CloseBes)
                {
                    var sarmayehItem = ItemList.Where(m => m.SarfaslId == (short)SarfaslEnums.Sarmayeh);
                    if (sarmayehItem.Any())
                    {
                        foreach (var item in sarmayehItem)
                        {
                            ItemList.Remove(item);
                        }
                    }
                    decimal temp = CloseBes - CloseBed;
                    ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = (short)SarfaslEnums.Sarmayeh, TafsiliId = null, Description = "".GetFiscalYearDescription(6, ""), Bedeh = temp, Bestan = 0 });
                    ItemList.Add(new SanadItem { SandId = 0, PeriodId = NewFiscalId, SarfaslId = (short)SarfaslEnums.Sarmayeh, TafsiliId = null, Description = "".GetFiscalYearDescription(7, ""), Bedeh = 0, Bestan = temp });
                    TotalAmount = CloseBes;

                }
                else if (CloseBed > CloseBes)
                {
                    var sarmayehItem = ItemList.Where(m => m.SarfaslId == (short)SarfaslEnums.Sarmayeh);
                    if (sarmayehItem.Any())
                    {
                        foreach (var item in sarmayehItem)
                        {
                            ItemList.Remove(item);
                        }
                    }
                    decimal temp = CloseBed - CloseBes;
                    ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = (short)SarfaslEnums.Sarmayeh, TafsiliId = null, Description = "".GetFiscalYearDescription(6, ""), Bedeh = 0, Bestan = temp });
                    ItemList.Add(new SanadItem { SandId = 0, PeriodId = NewFiscalId, SarfaslId = (short)SarfaslEnums.Sarmayeh, TafsiliId = null, Description = "".GetFiscalYearDescription(7, ""), Bedeh = temp, Bestan = 0 });
                    TotalAmount = CloseBed;
                }
                else
                {
                    TotalAmount = CloseBed;
                }

                var TavazonSanad = _blFiscalYearOp.GetSanadObject(TavazonSanadId, FiscalId, "".GetFiscalYearDescription(5, ""), (byte)SandType.TavazonSanad, 1, date, TavazonBed);
                var CloseSanad = _blFiscalYearOp.GetSanadObject(CloseSanadId, FiscalId, "".GetFiscalYearDescription(8, ""), (byte)SandType.CloseSanad, 1, date, TotalAmount); ;
                var openSanad = _blFiscalYearOp.GetSanadObject(OpenSanadId, NewFiscalId, "".GetFiscalYearDescription(9, ""), (byte)SandType.OpenSanad, 1, date, TotalAmount); ;
                if (_blFiscalYearOp.CloseFinanYearOp(TavazonItem, ItemList, Fiscal, CloseSanad, TavazonSanad, openSanad, FiscalId))
                {
                    if (Request.Cookies.AllKeys.Contains("CurrentFiscalYear"))
                    {
                        //Request.Cookies["CurrentFiscalYear"].Value=NewFiscalId.ToString();
                        Response.Cookies["CurrentFiscalYear"].Expires = DateTime.Now.AddDays(-1);
                        Request.Cookies.Remove("CurrentFiscalYear");
                        var cookie = new HttpCookie("CurrentFiscalYear", NewFiscalId.ToString());
                        cookie.Expires = Fiscal.EndDate;
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                    }
                    TempData["IndexMessage"] = Notification.Show("سال مالی با موفقیت بسته شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return Json(new JsonData
                    {
                        Success = true,
                        Html = Url.Action("Index", "Home")
                    });
                }
                else
                {
                    return Json(new JsonData
                    {
                        Success = false,
                        Script = Notification.BriefShow("خطا در بستن سال مالی", type: ToastType.Error)
                    });
                }
            }
            else
            {
                return Json(new JsonData
                {
                    Success = false,
                    Script = Notification.BriefShow("خطا در بستن سال مالی", type: ToastType.Error)
                });
            }
        }

        #region CloseCashdesk
        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult CashDeskFiscalYearOp(short FiscalId)
        {
            short CashDeskSarfasl = (short)SarfaslEnums.CashDesk;
            var TafisiliId = _blTafsili.Where(m => m.SarfaslId == CashDeskSarfasl).ToList();
            short nextFiscal = (short)(FiscalId + 1);
            List<SanadItem> ItemList = new List<SanadItem>();
            List<int[]> Reviewed = new List<int[]>();
            decimal TotalBed = 0;
            decimal TotalBes = 0;
            if (Session[FiscalYearItem] != null)
            {

                ItemList = (List<SanadItem>)Session[FiscalYearItem];
                Reviewed = (List<int[]>)Session[ReviewedItem];
            }
            foreach (var item in TafisiliId)
            {
                int id = item.Id;
                if (!Reviewed.Any(m => m.SequenceEqual(new int[] { id, 0 })))
                {
                    int Result = _blReport.GetTotalResultByTafsili(id, FiscalId, ref TotalBed, ref TotalBes);

                    if (TotalBed >= TotalBes)
                    {
                        if (TotalBed != TotalBes)
                        {
                            decimal temp = TotalBed - TotalBes;
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = CashDeskSarfasl, TafsiliId = id, Description = item.TafsiliName.GetFiscalYearDescription(1, "صندوق"), Bedeh = 0, Bestan = temp });
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = CashDeskSarfasl, TafsiliId = id, Description = item.TafsiliName.GetFiscalYearDescription(2, "صندوق"), Bedeh = temp, Bestan = 0 });
                        }
                        Reviewed.Add(new int[] { id, 0 });
                    }
                    else
                    {
                        return Json(new JsonData
                        {
                            Success = false,
                            Script = Notification.BriefShow("ماهیت صندوق" + "/" + item.TafsiliName + " " + "باید بدهکار باشد", type: ToastType.Warning)
                        });
                    }
                }
            }
            Session[FiscalYearItem] = ItemList;
            Session[ReviewedItem] = Reviewed;
            return Json(new JsonData
            {
                Success = true,
                Script = Notification.BriefShow("حساب صندوق ها با موفقیت بسته شد", type: ToastType.Success)
            });
        }
        #endregion

        #region CloseBank
        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult BankFiscalYearOp(short FiscalId)
        {
            short Bank = (short)SarfaslEnums.Bank;
            var TafisiliId = _blTafsili.Where(m => m.SarfaslId == Bank).ToList();
            short nextFiscal = (short)(FiscalId + 1);
            List<SanadItem> ItemList = new List<SanadItem>();
            List<int[]> Reviewed = new List<int[]>();
            decimal TotalBed = 0;
            decimal TotalBes = 0;
            if (Session[FiscalYearItem] != null)
            {

                ItemList = (List<SanadItem>)Session[FiscalYearItem];
                Reviewed = (List<int[]>)Session[ReviewedItem];
            }
            foreach (var item in TafisiliId)
            {
                int id = item.Id;
                if (!Reviewed.Any(m => m.SequenceEqual(new int[] { id, 0 })))
                {
                    int Result = _blReport.GetTotalResultByTafsili(id, FiscalId, ref TotalBed, ref TotalBes);

                    if (TotalBed >= TotalBes)
                    {
                        if (TotalBed != TotalBes)
                        {
                            decimal temp = TotalBed - TotalBes;
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = Bank, TafsiliId = id, Description = item.TafsiliName.GetFiscalYearDescription(1, "بانک"), Bedeh = 0, Bestan = temp });
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = Bank, TafsiliId = id, Description = item.TafsiliName.GetFiscalYearDescription(2, "بانک"), Bedeh = temp, Bestan = 0 });
                        }
                        Reviewed.Add(new int[] { id, 0 });
                    }
                    else
                    {
                        return Json(new JsonData
                        {
                            Success = false,
                            Script = Notification.BriefShow("ماهیت بانک" + "/" + item.TafsiliName + " " + "باید بدهکار باشد", type: ToastType.Warning)
                        });
                    }
                }
            }
            Session[FiscalYearItem] = ItemList;
            Session[ReviewedItem] = Reviewed;
            return Json(new JsonData
            {
                Success = true,
                Script = Notification.BriefShow("حساب بانک ها با موفقیت بسته شد", type: ToastType.Success)
            });
        }
        #endregion


        #region CloseHesabSarmayeh
        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult HesabSarmayehFiscalYearOp(short FiscalId)
        {
            short HesabSarmayeh = (short)SarfaslEnums.Sarmayeh;
            short nextFiscal = (short)(FiscalId + 1);
            List<SanadItem> ItemList = new List<SanadItem>();
            List<int[]> Reviewed = new List<int[]>();
            decimal TotalBed = 0;
            decimal TotalBes = 0;
            if (Session[FiscalYearItem] != null)
            {

                ItemList = (List<SanadItem>)Session[FiscalYearItem];
                Reviewed = (List<int[]>)Session[ReviewedItem];
            }

            if (!Reviewed.Any(m => m.SequenceEqual(new int[] { HesabSarmayeh, 5 })))
            {
                int Result = _blReport.GetTotalResultBySarfasl(HesabSarmayeh,FiscalId,ref TotalBed,ref TotalBes);

                if (TotalBed >= TotalBes)
                {
                    if (TotalBed != TotalBes)
                    {
                        decimal temp = TotalBed - TotalBes;
                        ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabSarmayeh, TafsiliId = null, Description = "".GetFiscalYearDescription(6, ""), Bedeh = 0, Bestan = temp });
                        ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = HesabSarmayeh, TafsiliId = null, Description ="".GetFiscalYearDescription(7, ""), Bedeh = temp, Bestan = 0 });
                    }
                   
                }
                else
                {
                    decimal temp = TotalBed - TotalBes;
                    ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabSarmayeh, TafsiliId = null, Description = "".GetFiscalYearDescription(6, ""), Bedeh = temp, Bestan =0  });
                    ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = HesabSarmayeh, TafsiliId = null, Description = "".GetFiscalYearDescription(7, ""), Bedeh =0 , Bestan = temp });
                }
                Reviewed.Add(new int[] { HesabSarmayeh, 5 });
            }
            Session[FiscalYearItem] = ItemList;
            Session[ReviewedItem] = Reviewed;
            return Json(new JsonData
            {
                Success = true,
                Script = Notification.BriefShow("حساب سرمایه با موفقیت بسته شد", type: ToastType.Success)
            });
        }
        #endregion

        #region HesabAshkhasFiscalYearOp

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult HesabAshkhasFiscalYearOp(short FiscalId)
        {
            short HesabDaryaftani = (short)SarfaslEnums.HesabDaryaftani;
            short HesabPardakhtani = (short)SarfaslEnums.HesabPardakhtani;
            var TafisiliId = _blTafsili.Where(m => m.SarfaslId == HesabDaryaftani).ToList();
            short nextFiscal = (short)(FiscalId + 1);
            List<SanadItem> ItemList = new List<SanadItem>();
            List<SanadItem> Tavazon = new List<SanadItem>();
            List<int[]> Reviewed = new List<int[]>();
            decimal TotalBedHD = 0;
            decimal TotalBesHD = 0;
            decimal TotalBedHP = 0;
            decimal TotalBesHP = 0;
            if (Session[FiscalYearItem] != null)
            {
                ItemList = (List<SanadItem>)Session[FiscalYearItem];
                Reviewed = (List<int[]>)Session[ReviewedItem];
                if (Session[TavazonList] != null)
                    Tavazon = (List<SanadItem>)Session[TavazonList];
            }

            foreach (var item in TafisiliId)
            {
                int Daryaftani = item.Id;
                int Pardakhtanti = _blReport.GetTafsiliId(item.PersonnelId, item.CustomerId, item.SupplierId, item.PersonTypeId);
                if (!Reviewed.Any(m => m.SequenceEqual(new int[] { Daryaftani, Pardakhtanti, 2 })))
                {
                    int RDaryaftani = _blReport.GetTotalResultByTafsili(Daryaftani, FiscalId, ref TotalBedHD, ref TotalBesHD);
                    int RPardakhtani = _blReport.GetTotalResultByTafsili(Pardakhtanti, FiscalId, ref TotalBedHP, ref TotalBesHP);

                    if (TotalBedHD < TotalBesHD && TotalBedHP > TotalBesHP)
                    {
                        //دریافتنی بستانکار و پرداختنی بدهکار شده است
                        if (TotalBesHD > TotalBedHP)
                        {
                            //دریافتنی متوازن خواهد شد
                            decimal temp = TotalBesHD - TotalBedHD;
                            Tavazon.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabDaryaftani, TafsiliId = Daryaftani, Description = item.TafsiliName.GetFiscalYearDescription(5, ""), Bedeh = temp, Bestan = 0 });
                            Tavazon.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabPardakhtani, TafsiliId = Pardakhtanti, Description = item.TafsiliName.GetFiscalYearDescription(5, ""), Bedeh = 0, Bestan = temp });
                        }
                        else
                        {
                            //پرداختنی متوازن خواهد شد
                            decimal temp = TotalBedHP - TotalBesHP;
                            Tavazon.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabPardakhtani, TafsiliId = Pardakhtanti, Description = item.TafsiliName.GetFiscalYearDescription(5, ""), Bedeh = 0, Bestan = temp });
                            Tavazon.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabDaryaftani, TafsiliId = Daryaftani, Description = item.TafsiliName.GetFiscalYearDescription(5, ""), Bedeh = temp, Bestan = 0 });
                        }
                    }
                    else if (TotalBesHP < TotalBedHP)
                    {
                        //پرداختنی بدهکار شده است
                        decimal temp = TotalBedHP - TotalBesHP;
                        Tavazon.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabPardakhtani, TafsiliId = Pardakhtanti, Description = item.TafsiliName.GetFiscalYearDescription(5, ""), Bedeh = 0, Bestan = temp });
                        Tavazon.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabDaryaftani, TafsiliId = Daryaftani, Description = item.TafsiliName.GetFiscalYearDescription(5, ""), Bedeh = temp, Bestan = 0 });
                    }
                    else if (TotalBedHD < TotalBesHD)
                    {
                        //دریافتنی بستانکار شده است
                        decimal temp = TotalBesHD - TotalBedHD;
                        Tavazon.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabDaryaftani, TafsiliId = Daryaftani, Description = item.TafsiliName.GetFiscalYearDescription(5, ""), Bedeh = temp, Bestan = 0 });
                        Tavazon.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabPardakhtani, TafsiliId = Pardakhtanti, Description = item.TafsiliName.GetFiscalYearDescription(5, ""), Bedeh = 0, Bestan = temp });
                    }

                    var DaryaftaniTvazonItem = Tavazon.Where(m => m.TafsiliId == Daryaftani);
                    if (DaryaftaniTvazonItem.Any())
                    {
                        decimal Bed = DaryaftaniTvazonItem.Sum(m => m.Bedeh);
                        decimal Bes = DaryaftaniTvazonItem.Sum(m => m.Bestan);
                        TotalBedHD += Bed;
                        TotalBesHD += Bes;
                    }
                    decimal tempDaryaftani = TotalBedHD - TotalBesHD;
                    if (tempDaryaftani != 0)
                    {
                        ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabDaryaftani, TafsiliId = Daryaftani, Description = item.TafsiliName.GetFiscalYearDescription(1, "حساب های دریافتنی"), Bedeh = 0, Bestan = tempDaryaftani });
                        ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = HesabDaryaftani, TafsiliId = Daryaftani, Description = item.TafsiliName.GetFiscalYearDescription(2, "حساب های دریافتنی"), Bedeh = tempDaryaftani, Bestan = 0 });
                    }
                    var PardakhtaniTvazonItem = Tavazon.Where(m => m.TafsiliId == Pardakhtanti);
                    if (DaryaftaniTvazonItem.Any())
                    {
                        decimal Bed = PardakhtaniTvazonItem.Sum(m => m.Bedeh);
                        decimal Bes = PardakhtaniTvazonItem.Sum(m => m.Bestan);
                        TotalBedHP += Bed;
                        TotalBesHP += Bes;
                    }
                    decimal tempPardakhtani = TotalBesHP - TotalBedHP;
                    if (tempPardakhtani != 0)
                    {
                        ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = HesabPardakhtani, TafsiliId = Pardakhtanti, Description = item.TafsiliName.GetFiscalYearDescription(1, "حساب های پرداختنی"), Bedeh = tempPardakhtani, Bestan = 0 });
                        ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = HesabPardakhtani, TafsiliId = Pardakhtanti, Description = item.TafsiliName.GetFiscalYearDescription(2, "حساب های پرداختنی"), Bedeh = 0, Bestan = tempPardakhtani });
                    }
                    Reviewed.Add(new int[] { Daryaftani, Pardakhtanti, 2 });
                }
            }

            Session[FiscalYearItem] = ItemList;
            Session[ReviewedItem] = Reviewed;
            Session[TavazonList] = Tavazon;
            return Json(new JsonData
            {
                Success = true,
                Script = Notification.BriefShow("حساب حساب های دریافتنی با موفقیت بسته شد", type: ToastType.Success)
            });
        }
        #endregion

        #region CloseAsnadDaryaftani
        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult AsnadDaryaftaniFiscalYearOp(short FiscalId)
        {
            short AsanadDaryaftani = (short)SarfaslEnums.AsnadDaryftani;
            var TafisiliId = _blTafsili.Where(m => m.SarfaslId == AsanadDaryaftani).ToList();
          
            short nextFiscal = (short)(FiscalId + 1);
            List<SanadItem> ItemList = new List<SanadItem>();
            List<int[]> Reviewed = new List<int[]>();
            decimal TotalBed = 0;
            decimal TotalBes = 0;
            if (Session[FiscalYearItem] != null)
            {

                ItemList = (List<SanadItem>)Session[FiscalYearItem];
                Reviewed = (List<int[]>)Session[ReviewedItem];
            }
            foreach (var item in TafisiliId)
            {
                int id = item.Id;
                if (!Reviewed.Any(m => m.SequenceEqual(new int[] { id, 0 })))
                {
                    int Result = _blReport.GetTotalResultByTafsili(id, FiscalId, ref TotalBed, ref TotalBes);

                    if (TotalBed >= TotalBes)
                    {
                        if (TotalBed != TotalBes)
                        {
                            decimal temp = TotalBed - TotalBes;
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = AsanadDaryaftani, TafsiliId = id, Description = item.TafsiliName.GetFiscalYearDescription(1, "اسناد دریافتنی"), Bedeh = 0, Bestan = temp });
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = AsanadDaryaftani, TafsiliId = id, Description = item.TafsiliName.GetFiscalYearDescription(2, "اسناد دریافتنی"), Bedeh = temp, Bestan = 0 });
                        }
                        Reviewed.Add(new int[] { id, 0 });
                    }
                    else
                    {
                        return Json(new JsonData
                        {
                            Success = false,
                            Script = Notification.BriefShow("ماهیت اسناد دریافتنی" + "/" + item.TafsiliName + " " + "باید بدهکار باشد", type: ToastType.Warning)
                        });
                    }
                    Reviewed.Add(new int[] { id, 0 });
                }
            }
            Session[FiscalYearItem] = ItemList;
            Session[ReviewedItem] = Reviewed;
            return Json(new JsonData
            {
                Success = true,
                Script = Notification.BriefShow("حساب اسناد دریافتنی با موفقیت بسته شد", type: ToastType.Success)
            });
        }
        #endregion

        #region CloseChequeDarJarayan
        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult ChequeDarJarayanFiscalYearOp(short FiscalId)
        {
            short ChequeDarJarayan = (short)SarfaslEnums.ChequeHayeDarJaryaneVosool;
            var TafisiliId = _blTafsili.Where(m => m.SarfaslId == ChequeDarJarayan).ToList();
          
            short nextFiscal = (short)(FiscalId + 1);
            List<SanadItem> ItemList = new List<SanadItem>();
            List<int[]> Reviewed = new List<int[]>();
            decimal TotalBed = 0;
            decimal TotalBes = 0;
            if (Session[FiscalYearItem] != null)
            {

                ItemList = (List<SanadItem>)Session[FiscalYearItem];
                Reviewed = (List<int[]>)Session[ReviewedItem];
            }
            foreach (var item in TafisiliId)
            {
                int id = item.Id;
                if (!Reviewed.Any(m => m.SequenceEqual(new int[] { id, 0 })))
                {
                    int Result = _blReport.GetTotalResultByTafsili(id, FiscalId, ref TotalBed, ref TotalBes);

                    if (TotalBed >= TotalBes)
                    {
                        if (TotalBed != TotalBes)
                        {
                            decimal temp = TotalBed - TotalBes;
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = ChequeDarJarayan, TafsiliId = id, Description = item.TafsiliName.GetFiscalYearDescription(1, "چک های در جریان در وصول"), Bedeh = 0, Bestan = temp });
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = ChequeDarJarayan, TafsiliId = id, Description = item.TafsiliName.GetFiscalYearDescription(2, "چک های در جریان در وصول"), Bedeh = temp, Bestan = 0 });
                        }

                    }
                    else
                    {
                        return Json(new JsonData
                        {
                            Success = false,
                            Script = Notification.BriefShow("ماهیت چک های در جریان وصول" + "/" + item.TafsiliName + " " + "باید بدهکار باشد", type: ToastType.Warning)
                        });
                    }
                    Reviewed.Add(new int[] { id, 0 });
                }
            }
            Session[FiscalYearItem] = ItemList;
            Session[ReviewedItem] = Reviewed;
            return Json(new JsonData
            {
                Success = true,
                Script = Notification.BriefShow("حساب چک های در جریان وصول با موفقیت بسته شد", type: ToastType.Success)
            });
        }
        #endregion

        #region CloseOtherDaraei
        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult OtherDaraeiFiscaYearOp(short FiscalId)
        {
            var OtherDaraei = _blReport.GetOtherDaraei();
           
            short nextFiscal = (short)(FiscalId + 1);
            List<SanadItem> ItemList = new List<SanadItem>();
            List<int[]> Reviewed = new List<int[]>();
            decimal TotalBed = 0;
            decimal TotalBes = 0;
            if (Session[FiscalYearItem] != null)
            {
                ItemList = (List<SanadItem>)Session[FiscalYearItem];
                Reviewed = (List<int[]>)Session[ReviewedItem];
            }
            foreach (var item in OtherDaraei)
            {
                var Tafsilis = _blTafsili.Where(m => m.SarfaslId == item.Id).ToList();
                if (Tafsilis.Count > 0)
                {
                    foreach (var Ti in Tafsilis)
                    {
                        int id = Ti.Id;
                        if (!Reviewed.Any(m => m.SequenceEqual(new int[] { id, 0 })))
                        {
                            int Result = _blReport.GetTotalResultByTafsili(id, FiscalId, ref TotalBed, ref TotalBes);
                            if (TotalBed >= TotalBes)
                            {
                                if (TotalBed != TotalBes)
                                {
                                    decimal temp = TotalBed - TotalBes;
                                    ItemList.Add(new SanadItem { SandId = 6, PeriodId = FiscalId, SarfaslId = Ti.SarfaslId.Value, TafsiliId = id, Description = Ti.TafsiliName.GetFiscalYearDescription(1, item.Name), Bedeh = 0, Bestan = temp });
                                    ItemList.Add(new SanadItem { SandId = 6, PeriodId = nextFiscal, SarfaslId = Ti.SarfaslId.Value, TafsiliId = id, Description = Ti.TafsiliName.GetFiscalYearDescription(2, item.Name), Bedeh = temp, Bestan = 0 });
                                }
                                Reviewed.Add(new int[] { id, 0 });
                            }
                            else
                            {


                                return Json(new JsonData
                                {
                                    Success = false,
                                    Script = Notification.BriefShow("ماهیت" + " " + item.Name + "/" + Ti.TafsiliName + " " + "باید بدهکار باشد", type: ToastType.Warning)
                                });
                            }
                        }
                    }
                }
                int Saraslid = item.Id;
                if (!Reviewed.Any(m => m.SequenceEqual(new int[] { Saraslid, 1 })))
                {
                    int Result = _blReport.GetTotalResultByTafsili(Saraslid, FiscalId, ref TotalBed, ref TotalBes);
                    if (TotalBed >= TotalBes)
                    {
                        if (TotalBed != TotalBes)
                        {
                            decimal temp = TotalBed - TotalBes;
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = item.Id, TafsiliId = null, Description = item.Name.GetFiscalYearDescription(3, item.Name), Bedeh = 0, Bestan = temp });
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = item.Id, TafsiliId = null, Description = item.Name.GetFiscalYearDescription(4, item.Name), Bedeh = temp, Bestan = 0 });
                        }

                    }
                    else
                    {

                        //if (TotalBes != TotalBed)
                        //{
                        //    decimal temp = TotalBes - TotalBed;
                        //    ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = item.Id, TafsiliId = null, Description = item.Name.GetFiscalYearDescription(3, item.Name), Bedeh = temp, Bestan = 0 });
                        //    ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = item.Id, TafsiliId = null, Description = item.Name.GetFiscalYearDescription(4, item.Name), Bedeh = 0, Bestan = temp });
                        //}
                        return Json(new JsonData
                        {
                            Success = false,
                            Script = Notification.BriefShow("ماهیت" + " " + item.Name + " " + "باید بدهکار باشد", type: ToastType.Warning)
                        });
                    }
                    Reviewed.Add(new int[] { Saraslid, 1 });
                }

            }
            Session[FiscalYearItem] = ItemList;
            Session[ReviewedItem] = Reviewed;

            return Json(new JsonData
            {
                Success = true,
                Script = Notification.BriefShow("حساب سایر دارایی ها با موفقیت بسته شد", type: ToastType.Success)
            });

        }
        #endregion
        
        #region CloseAsnadPardakhtani
        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult AsnadPardakhtaniFiscalYearOp(short FiscalId)
        {
            short AsnadPardakhtani = (short)SarfaslEnums.AsnadPardakhati;
            var TafisiliId = _blTafsili.Where(m => m.SarfaslId == AsnadPardakhtani).ToList();
            short nextFiscal = (short)(FiscalId + 1);
            List<SanadItem> ItemList = new List<SanadItem>();
            List<int[]> Reviewed = new List<int[]>();
            decimal TotalBed = 0;
            decimal TotalBes = 0;
            if (Session[FiscalYearItem] != null)
            {

                ItemList = (List<SanadItem>)Session[FiscalYearItem];
                Reviewed = (List<int[]>)Session[ReviewedItem];
            }
            foreach (var item in TafisiliId)
            {
                int id = item.Id;
                if (!Reviewed.Any(m => m.SequenceEqual(new int[] { id, 0 })))
                {
                    int Result = _blReport.GetTotalResultByTafsili(id, FiscalId, ref TotalBed, ref TotalBes);

                    if (TotalBed <= TotalBes)
                    {
                        if (TotalBes != TotalBed)
                        {
                            decimal temp = TotalBes - TotalBed;
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = AsnadPardakhtani, TafsiliId = id, Description = item.TafsiliName.GetFiscalYearDescription(1, "اسناد پرداختنی"), Bedeh = temp, Bestan = 0 });
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = AsnadPardakhtani, TafsiliId = id, Description = item.TafsiliName.GetFiscalYearDescription(2, "اسناد پرداختنی"), Bedeh = 0, Bestan = temp });
                        }
                        Reviewed.Add(new int[] { id, 0 });
                    }
                    else
                    {
                        return Json(new JsonData
                        {
                            Success = false,
                            Script = Notification.BriefShow("ماهیت اسناد پرداختنی" + "/" + item.TafsiliName + " " + "باید بستانکار باشد", type: ToastType.Warning)
                        });
                    }
                }
            }
            Session[FiscalYearItem] = ItemList;
            Session[ReviewedItem] = Reviewed;
            return Json(new JsonData
            {
                Success = true,
                Script = Notification.BriefShow("حساب اسناد پرداختنی با موفقیت بسته شد", type: ToastType.Success)
            });
        }
        #endregion

        #region CloseOtherBedhi
        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.CloseFiscalPeriod)]
        public ActionResult OtherBedhiFiscaYearOp(short FiscalId)
        {
            var OtherBedhi = _blReport.GetOtherBedhi();
          
            short nextFiscal = (short)(FiscalId + 1);
            List<SanadItem> ItemList = new List<SanadItem>();
            List<int[]> Reviewed = new List<int[]>();
            decimal TotalBed = 0;
            decimal TotalBes = 0;
            if (Session[FiscalYearItem] != null)
            {
                ItemList = (List<SanadItem>)Session[FiscalYearItem];
                Reviewed = (List<int[]>)Session[ReviewedItem];
            }
            foreach (var item in OtherBedhi)
            {
                var Tafsilis = _blTafsili.Where(m => m.SarfaslId == item.Id).ToList();
                if (Tafsilis.Count > 0)
                {
                    foreach (var Ti in Tafsilis)
                    {
                        int id = Ti.Id;
                        if (!Reviewed.Any(m => m.SequenceEqual(new int[] { id, 0 })))
                        {
                            int Result = _blReport.GetTotalResultByTafsili(id, FiscalId, ref TotalBed, ref TotalBes);
                            if (TotalBed <= TotalBes)
                            {
                                if (TotalBes != TotalBed)
                                {
                                    decimal temp = TotalBes - TotalBed;
                                    ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = Ti.SarfaslId.Value, TafsiliId = id, Description = Ti.TafsiliName.GetFiscalYearDescription(1, item.Name), Bedeh = temp, Bestan = 0 });
                                    ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = Ti.SarfaslId.Value, TafsiliId = id, Description = Ti.TafsiliName.GetFiscalYearDescription(2, item.Name), Bedeh = 0, Bestan = temp });
                                }

                            }
                            else
                            {
                                return Json(new JsonData
                                {
                                    Success = false,
                                    Script = Notification.BriefShow("ماهیت" + " " + item.Name + "/" + Ti.TafsiliName + " " + "باید بستانکار باشد", type: ToastType.Warning)
                                });
                            }
                            Reviewed.Add(new int[] { id, 0 });
                        }
                    }
                }
                int Saraslid = item.Id;
                if (!Reviewed.Any(m => m.SequenceEqual(new int[] { Saraslid, 1 })))
                {
                    int Result = _blReport.GetTotalResultByTafsili(Saraslid, FiscalId, ref TotalBed, ref TotalBes);
                    if (TotalBed <= TotalBes)
                    {
                        if (TotalBes != TotalBed)
                        {
                            decimal temp = TotalBes - TotalBed;
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = item.Id, TafsiliId = null, Description = item.Name.GetFiscalYearDescription(3, item.Name), Bedeh = temp, Bestan = 0 });
                            ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = item.Id, TafsiliId = null, Description = item.Name.GetFiscalYearDescription(4, item.Name), Bedeh = 0, Bestan = temp });
                        }
                        Reviewed.Add(new int[] { Saraslid, 1 });
                    }
                    else
                    {
                        //if (TotalBes != TotalBed)
                        //{
                        //    decimal temp = TotalBed - TotalBes;
                        //    ItemList.Add(new SanadItem { SandId = 0, PeriodId = FiscalId, SarfaslId = item.Id, TafsiliId = null, Description = item.Name.GetFiscalYearDescription(3, item.Name), Bedeh = 0, Bestan = temp });
                        //    ItemList.Add(new SanadItem { SandId = 0, PeriodId = nextFiscal, SarfaslId = item.Id, TafsiliId = null, Description = item.Name.GetFiscalYearDescription(4, item.Name), Bedeh = 0, Bestan = temp });
                        //}
                        return Json(new JsonData
                        {
                            Success = false,
                            Script = Notification.BriefShow("ماهیت" + " " + item.Name + " " + "باید بستانکار باشد", type: ToastType.Warning)
                        });
                    }
                }

            }
            Session[FiscalYearItem] = ItemList;
            Session[ReviewedItem] = Reviewed;

            return Json(new JsonData
            {
                Success = true,
                Script = Notification.BriefShow("حساب سایر بدهی ها با موفقیت بسته شد", type: ToastType.Success)
            });

        }
        #endregion

        #endregion
   
    }
}