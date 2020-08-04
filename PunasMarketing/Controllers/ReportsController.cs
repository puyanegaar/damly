using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PunasMarketing.Models.Enums;

namespace PunasMarketing.Controllers
{
    public class ReportsController : Controller
    {
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

        public ReportsController(
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
            _blReport = reportsRepository;
            _blFiscal = fiscalRepository;
        }
        #endregion

        #region journalReport

        [HttpGet]
        [AccessControl(ActionsEnum.ShowJournalReport)]
        public ActionResult JournalReport()
        {
            var model = new journalReportViewModel();
            var startDate = DateTime.Now.GetStartDate();
            var EndDate = DateTime.Now.GetEndDate();
            model.StartDate = startDate.ToPersianDateTime().ToString("yyyy/MM/dd");
            model.EndDate = EndDate.ToPersianDateTime().ToString("yyyy/MM/dd");
            model.JournalList = _blReport.GetJournalList(Fiscal.GetFiscalId(), startDate, EndDate);
            return View(model);
        }

        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetJournalList(string FromDate, string ToDate)
        {
            var model = new journalReportViewModel();
            var startDate = FromDate.ToMiladiDate().GetStartDate();
            var EndDate = ToDate.ToMiladiDate().GetEndDate();
            model.StartDate = FromDate;
            model.EndDate = ToDate;
            model.JournalList = _blReport.GetJournalList(Fiscal.GetFiscalId(), startDate, EndDate);
            return PartialView("_JournalList", model);
        }
        #endregion

        #region journalTotalAccountsReport

        [HttpGet]
        [AccessControl(ActionsEnum.ShowJournalTotalAccountsReport)]
        public ActionResult journalTotalAccountsReport()
        {
            var model = new journalReportViewModel();
            var startDate = DateTime.Now.GetStartDate();
            var EndDate = DateTime.Now.GetEndDate();
            model.StartDate = startDate.ToPersianDateTime().ToString("yyyy/MM/dd");
            model.EndDate = EndDate.ToPersianDateTime().ToString("yyyy/MM/dd");
            var list = _blReport.GetJournalTAList(Fiscal.GetFiscalId(), startDate, EndDate).ToList();
            model.JournalTAList = list;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["TableRowSize"]);
            model.TotalItem = list.Count;
            int pageCout = (int)Math.Ceiling((decimal)(model.TotalItem / pageSize));
            model.TotalPage = pageCout == 0 ? 1 : pageCout;
            return View(model);
        }

        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetjournalTotalAccountsList(string FromDate, string ToDate)
        {
            var model = new journalReportViewModel();
            var startDate = FromDate.ToMiladiDate().GetStartDate();
            var EndDate = ToDate.ToMiladiDate().GetEndDate();
            model.StartDate = FromDate;
            model.EndDate = ToDate;
            var list = _blReport.GetJournalTAList(Fiscal.GetFiscalId(), startDate, EndDate).ToList();
            model.JournalTAList = list;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["TableRowSize"]);
            model.TotalItem = list.Count;
            float size = (float)model.TotalItem / (float)pageSize;
            int pageCout = (int)Math.Ceiling(size);
            model.TotalPage = pageCout == 0 ? 1 : pageCout;
            return PartialView("_JournalTAList", model);
        }
        #endregion

        #region ledgerReport
        [HttpGet]
        [AccessControl(ActionsEnum.LedgerReport)]
        public ActionResult LedgerReport()
        {
            var model = new LedgerReportViewModel();
            var fiscal = _blFiscal.Find(Fiscal.GetFiscalId());
            model.StartDate = fiscal.StartDate.ToPersianDateTime().ToString("yyyy/MM/dd");
            model.EndDate = fiscal.EndDate.ToPersianDateTime().ToString("yyyy/MM/dd");
            model.Sarfasls = _blSarfasl.Select().ToList();
            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetTafsiliList(short SarfaslId, string SarfaslName)
        {
            var model = new LedgerReportViewModel();
            model.SarfaslName = SarfaslName;
            model.GetTForL = _blTafsili.Where(m => m.SarfaslId == SarfaslId);
            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_TafsiliResult", model)
            });
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetLadgerReport(byte type, string StartDate, string EndDate, short SarfaslId, int TafsiliId)
        {
            if (type != 0 && StartDate != "" && EndDate != "" && SarfaslId != 0 || TafsiliId != 0)
            {

                var model = new LedgerReportViewModel();
                short FiscalId = Fiscal.GetFiscalId();
                var fiscal = _blFiscal.Find(FiscalId);
                bool MondehAzGhable = false;
                decimal TotalBedeh = 0;
                decimal TotalBestan = 0;
                var Fdate = StartDate.ToMiladiDate().GetStartDate();
                var Tdate = EndDate.ToMiladiDate().GetStartDate();
                model.BeforeFromDate = Tdate.ToString();
                if (!fiscal.StartDate.Equals(Fdate))
                {
                    var FromDate = fiscal.StartDate;
                    var ToDate = Fdate.AddDays(-1).GetEndDate();
                    model.BeforeFromDate = ToDate.ToPersianDateTime().ToString("yyyy/MM/dd");
                    MondehAzGhable = true;
                    if (type == 1)
                        _blReport.GetMandehForLedgerReport(type, SarfaslId, FiscalId, FromDate, ToDate, null, ref TotalBedeh, ref TotalBestan);
                    else if (type == 2)
                        _blReport.GetMandehForLedgerReport(type, SarfaslId, FiscalId, FromDate, ToDate, null, ref TotalBedeh, ref TotalBestan);
                    else if (type == 3)
                        _blReport.GetMandehForLedgerReport(type, SarfaslId, FiscalId, FromDate, ToDate, TafsiliId, ref TotalBedeh, ref TotalBestan);
                }
                if (type == 1)
                    model.ledgerReport = _blReport.GetLedgerReport(type, SarfaslId, FiscalId, Fdate, Tdate, null);
                else if (type == 2)
                    model.ledgerReport = _blReport.GetLedgerReport(type, SarfaslId, FiscalId, Fdate, Tdate, null);
                else if (type == 3)
                    model.ledgerReport = _blReport.GetLedgerReport(type, SarfaslId, FiscalId, Fdate, Tdate, TafsiliId);
                model.TBed = TotalBedeh;
                model.TBes = TotalBestan;
                model.Mondeh = TotalBestan - TotalBedeh;
                model.MondehAzGhable = MondehAzGhable;
                return PartialView("_LedgerReportList", model);
            }
            else
            {
                return Json(new { Message = Notification.BriefShow("لطفا اطلاعات گزارش درخواستی را وارد نمایید!") });
            }

        }
        #endregion
    }
}