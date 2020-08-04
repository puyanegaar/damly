using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.LocalModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Transaction;
using PunasMarketing.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Controllers
{

    public class TransactionController : Controller
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
        private readonly FiscalRepository _blFiscal;

        public TransactionController(
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
        #endregion


        #region ReceivePart
        [HttpGet]
        [AccessControl(ActionsEnum.ShowTransactions)]
        public ActionResult Receives()
        {
            short fiscalId = Fiscal.GetFiscalId();
            return View(_blSanad.Where(i => i.SanadTypeId == (byte)SandType.Receive && i.PeriodId == fiscalId)
                .OrderByDescending(i => i.Id));
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteTransaction)]
        [FiscalCheck]
        [SanadUpdateCheck]
        public JsonResult DeleteTransaction(int id)
        {
            short fiscalId = Fiscal.GetFiscalId();

            if (_blSanad.Delete(id, fiscalId, out bool isUsed))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false, IsUsed = isUsed });
            }
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddTransaction)]
        [FiscalCheck]
        public ActionResult AddReceive()
        {
            PaymentViewModel model = new PaymentViewModel();
            model.Sarfasls = _blSarfasl.Where(m => m.IsMoeen == true).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Coding + "-" + s.Name });
            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.AddTransaction)]
        [FiscalCheck]
        public ActionResult AddReceive(PaymentViewModel PayModel)
        {
            if (ModelState.IsValid)
            {
                if (PayModel.cash == null && PayModel.Card == null && PayModel.cheques == null)
                {
                    return Notification.jsonShow("آیتمی برای این دریافت وجه / چک وارد نشده است", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
                int PersonId = int.Parse(PayModel.pay.Persons.ToString().Split('/')[0]);
                string PersonName = PayModel.pay.Persons.ToString().Split('/')[1];
                if (string.IsNullOrEmpty(PayModel.pay.Description))
                    PayModel.pay.Description = PersonName.GetRecevieDescription(4);
                short FiscalId = Fiscal.GetFiscalId();
                var date = DateTime.Now;
                PayModel.pay.FiscalId = FiscalId;
                PayModel.pay.SanadId = _blSanad.GetLastIdentity(FiscalId) + 1;
                if (_blSanad.AddReceive(PayModel.pay, date, UserIdentity.Getuserid()))
                {
                    int sandId = PayModel.pay.SanadId;
                    decimal Total = 0;
                    decimal tempTotal = 0;
                    int PersonHesab = 0;
                    short PersonSarfasl = 0;
                    if (PayModel.pay.PersonType == 1)
                    {
                        var p = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                        PersonHesab = p.Id;
                        PersonSarfasl = p.SarfaslId.Value;
                    }
                    else if (PayModel.pay.PersonType == 2)
                    {
                        var p = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                        PersonHesab = p.Id;
                        PersonSarfasl = p.SarfaslId.Value;
                    }
                    else
                    {
                        var p = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                        PersonHesab = p.Id;
                        PersonSarfasl = p.SarfaslId.Value;
                    }

                    if (PayModel.cash != null && PayModel.cash.Length > 0)
                    {
                        for (int i = 0; i < PayModel.cash.Length; i++)
                        {
                            short cId = PayModel.cash[i].CashDeskId;
                            var CashHesab = _blTafsili.Where(m => m.CashDeskId == cId && m.SarfaslId == (short)SarfaslEnums.CashDesk).FirstOrDefault();
                            _blSanadItem.Add(sandId, FiscalId, date, null, null, PersonSarfasl, PersonHesab, PersonName.GetRecevieDescription(1), 0, PayModel.cash[i].Amount.ConvertToPrice(), false);
                            _blSanadItem.Add(sandId, FiscalId, date, null, null, CashHesab.SarfaslId.Value, CashHesab.Id, PersonName.GetRecevieDescription(1), PayModel.cash[i].Amount.ConvertToPrice(), 0, false);
                            tempTotal += PayModel.cash[i].Amount.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.Card != null && PayModel.Card.Length > 0)
                    {
                        for (int i = 0; i < PayModel.Card.Length; i++)
                        {
                            short BId = PayModel.Card[i].BankAccountId;
                            var BankHesab = _blTafsili.Where(m => m.BankAccId == BId && m.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                            _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, PersonSarfasl, PersonHesab, PersonName.GetRecevieDescription(2, "", PayModel.Card[i].IssueTracking), 0, PayModel.Card[i].Amount.ConvertToPrice(), false);
                            _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, BankHesab.SarfaslId.Value, BankHesab.Id, PersonName.GetRecevieDescription(2, "", PayModel.Card[i].IssueTracking), PayModel.Card[i].Amount.ConvertToPrice(), 0, false);
                            tempTotal += PayModel.Card[i].Amount.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.cheques != null && PayModel.cheques.Length > 0)
                    {

                        var tafsiliAsnadD = new Tafsili();
                        if (PayModel.pay.PersonType == 1)
                        {
                            tafsiliAsnadD = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.AsnadDaryftani).FirstOrDefault();

                        }
                        else if (PayModel.pay.PersonType == 2)
                        {
                            tafsiliAsnadD = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.AsnadDaryftani).FirstOrDefault();

                        }
                        else
                        {
                            tafsiliAsnadD = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.AsnadDaryftani).FirstOrDefault();

                        }

                        for (int i = 0; i < PayModel.cheques.Length; i++)
                        {

                            PayModel.cheques[i].StatusId = 1;
                            PayModel.cheques[i].DueDate = PayModel.cheques[i].DudateText.ToMiladiDate();
                            PayModel.cheques[i].Amount = PayModel.cheques[i].Price.ConvertToPrice();
                            _blcheque.Add(PayModel.cheques[i]);
                            int ChequeId = _blcheque.GetLastIdentity();
                            PayModel.cheques[i].Id = _blcheque.GetLastIdentity();
                        }

                        for (int i = 0; i < PayModel.cheques.Length; i++)
                        {
                            _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, PersonSarfasl, PersonHesab, PersonName.GetRecevieDescription(3, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), 0, PayModel.cheques[i].Price.ConvertToPrice(), false);
                            _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, tafsiliAsnadD.SarfaslId.Value, tafsiliAsnadD.Id, PersonName.GetRecevieDescription(3, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), PayModel.cheques[i].Price.ConvertToPrice(), 0, false);
                            tempTotal += PayModel.cheques[i].Price.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;
                        }
                        tempTotal = 0;

                    }

                    if (Total != PayModel.pay.TotalAmount)
                    {
                        var sanad = _blSanad.Find(sandId, FiscalId);
                        sanad.TotalAmount = Total;
                        _blSanad.Update(sanad);
                    }

                    TempData["saveMassage"] = Notification.Show("ثبت دریافت وجه / چک با موفقیت انجام شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return Json(new { RedirectUrl = Url.Action("Receives", "Transaction") });
                }
                else
                {
                    return Notification.jsonShow("ثبت دریافت وجه / چک یا خطا رو به رو شد", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                return Notification.jsonShow(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);

            }

        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateTransaction)]
        [FiscalCheck]
        [SanadUpdateCheck]
        public ActionResult UpdateReceive(int? ReceiveId)
        {
            if (ReceiveId.HasValue)
            {
                var model = new PaymentViewModel();
                model.CashDesks = _blCashDesk.Where(m => m.IsAvailable == true);
                model.BankAccount = _blBank.SelectView();
                model.ChequeBank = _blBankName.Select();
                var pay = new PaymentModel();
                model.deletedItem = "0";
                model.deteledCheque = "0";
                short FiscalId = Fiscal.GetFiscalId();
                var sanad = _blSanad.Find(ReceiveId.Value, FiscalId);
                var sItems = sanad.SanadItems;
                var Cash = sItems.Where(m => m.IssueTrackingNum == null && m.ChequeId == null).ToArray();
                var Card = sItems.Where(m => m.IssueTrackingNum != null).ToArray();
                var Cheque = sItems.Where(m => m.ChequeId != null).ToArray();
                var Tafsili = sItems.FirstOrDefault().Tafsili;
                int PersonTypeId = (short)Tafsili.PersonTypeId;
                string personName = "";
                decimal bes = 0;
                decimal bed = 0;
                switch (PersonTypeId)
                {
                    case 1:
                        var personnel = Tafsili.Personnel;
                        personName = personnel.Id + "/" + personnel.FullName;
                        var PersonnelD = _blTafsili.Where(m => m.PersonnelId == personnel.Id && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                        var PersonnelP = _blTafsili.Where(m => m.PersonnelId == personnel.Id && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                        var bestanp = _blSanadItem.Where(m => (m.TafsiliId == PersonnelD.Id || m.TafsiliId == PersonnelP.Id) && m.SandId != ReceiveId);
                        var bedehp = _blSanadItem.Where(m => (m.TafsiliId == PersonnelD.Id || m.TafsiliId == PersonnelP.Id) && m.SandId != ReceiveId);
                        if (bestanp.Any())
                            bes = bestanp.Sum(m => m.Bestan);
                        else
                            bes = 0;

                        if (bedehp.Any())
                            bed = bedehp.Sum(m => m.Bedeh);
                        else
                            bes = 0;
                        break;
                    case 2:
                        var customer = Tafsili.Customer;
                        personName = customer.Id + "/" + customer.OwnerName;
                        var CustomerD = _blTafsili.Where(m => m.CustomerId == customer.Id && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                        var CustomerP = _blTafsili.Where(m => m.CustomerId == customer.Id && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                        var bestanc = _blSanadItem.Where(m => (m.TafsiliId == CustomerD.Id || m.TafsiliId == CustomerP.Id) && m.SandId != ReceiveId);
                        var bedehc = _blSanadItem.Where(m => (m.TafsiliId == CustomerD.Id || m.TafsiliId == CustomerP.Id) && m.SandId != ReceiveId);
                        if (bestanc.Any())
                            bes = bestanc.Sum(m => m.Bestan);
                        else
                            bes = 0;

                        if (bedehc.Any())
                            bed = bedehc.Sum(m => m.Bedeh);
                        else
                            bes = 0;
                        break;
                    case 3:
                        var supplier = Tafsili.Supplier;
                        personName = supplier.Id + "/" + supplier.Name;
                        var SupplierD = _blTafsili.Where(m => m.SupplierId == supplier.Id && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                        var SupplierP = _blTafsili.Where(m => m.SupplierId == supplier.Id && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                        var bestans = _blSanadItem.Where(m => (m.TafsiliId == SupplierD.Id || m.TafsiliId == SupplierP.Id) && m.SandId != ReceiveId);
                        var bedehs = _blSanadItem.Where(m => (m.TafsiliId == SupplierD.Id || m.TafsiliId == SupplierP.Id) && m.SandId != ReceiveId);
                        if (bestans.Any())
                            bes = bestans.Sum(m => m.Bestan);
                        else
                            bes = 0;

                        if (bedehs.Any())
                            bed = bedehs.Sum(m => m.Bedeh);
                        else
                            bes = 0;
                        break;
                };
                model.Taraz = bes - bed;
                #region sanadPart
                pay.PersonType = PersonTypeId;
                pay.Persons = personName;
                pay.Description = sanad.Description;
                pay.TotalAmount = sanad.TotalAmount;
                pay.SanadId = ReceiveId.Value;
                model.pay = pay;
                #endregion
                #region CashPart
                int CashCount = Cash.Count();
                if (CashCount > 0)
                {
                    CashModel[] cashes = new CashModel[CashCount / 2];
                    for (int i = 0; i < CashCount; i++)
                    {
                        int m = 0;
                        if ((i % 2) == 0)
                        {
                            if (i != 0)
                                m = i - 1;
                            CashModel cm = new CashModel();
                            cm.id = Cash[i].Id;
                            cm.SecondId = Cash[i + 1].Id;
                            cm.CashDeskId = Cash[i + 1].Tafsili.CashDeskId.Value;
                            cm.Amount = Cash[i].Bestan.ToPrice();
                            cashes[m] = cm;

                        }

                    }
                    model.EditCash = cashes;
                }
                #endregion
                #region CardPart
                int CardCount = Card.Count();
                if (CardCount > 0)
                {
                    CardModel[] Cards = new CardModel[CardCount / 2];
                    for (int i = 0; i < CardCount; i++)
                    {
                        int m = 0;
                        if ((i % 2) == 0)
                        {
                            if (i != 0)
                                m = i - 1;
                            CardModel cm = new CardModel();
                            cm.id = Card[i].Id;
                            cm.SecondId = Card[i + 1].Id;
                            var t = Card[i + 1].Tafsili;
                            cm.BankAccountId = t.BankAccId.Value;
                            var b = t.BankAccount;
                            cm.CardAccountNumber = b.AccountNum + "/" + b.Owner;
                            cm.Amount = Card[i].Bestan.ToPrice();
                            cm.IssueTracking = Card[i].IssueTrackingNum;
                            Cards[m] = cm;

                        }

                    }
                    model.EditCard = Cards;
                }
                #endregion
                #region ChequePart
                int ChequeCount = Cheque.Count();
                if (ChequeCount > 0)
                {
                    Cheque[] cheques = new Cheque[ChequeCount / 2];
                    for (int i = 0; i < ChequeCount; i++)
                    {
                        int m = 0;
                        if ((i % 2) == 0)
                        {
                            if (i != 0)
                                m = i - 1;
                            Cheque cm = new Cheque();
                            var ch = Cheque[i].Cheque;
                            cm.Id = ch.Id;
                            cm.ChequeNum = ch.ChequeNum;
                            cm.Price = ch.Amount.ToPrice();
                            cm.DudateText = ch.DueDate.ToPersianDateTime().ToString("yyyy/MM/dd");
                            cm.BankId = ch.BankId;
                            cm.Description = ch.Description;
                            cm.ChequeSanadId = Cheque[i].Id;
                            cm.ChequeSanadSecondId = Cheque[i + 1].Id;
                            cheques[m] = cm;

                        }

                    }
                    model.Editcheques = cheques;
                }
                #endregion
                return View(model);
            }
            else
            {
                //return To Not Found
            }
            return View();
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.UpdateTransaction)]
        [FiscalCheck]
        public ActionResult UpdateReceive(PaymentViewModel PayModel)
        {
            if (ModelState.IsValid)
            {
                if (PayModel.pay.TotalAmount == 0)
                {
                    return Notification.jsonShow("آیتمی برای ویرایش در این دریافت وجه / چک وجود ندارد", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
                int PersonId = int.Parse(PayModel.pay.Persons.ToString().Split('/')[0]);
                string PersonName = PayModel.pay.Persons.ToString().Split('/')[1];
                if (string.IsNullOrEmpty(PayModel.pay.Description))
                    PayModel.pay.Description = PersonName.GetRecevieDescription(4);
                short FiscalId = Fiscal.GetFiscalId();
                PayModel.pay.FiscalId = FiscalId;
                DateTime date = new DateTime();
                if (_blSanad.UpdateReceive(PayModel.pay, ref date))
                {
                    int sandId = PayModel.pay.SanadId;
                    decimal Total = 0;
                    decimal tempTotal = 0;
                    int PersonHesab = 0;
                    short PersonSarfasl = 0;
                    if (PayModel.pay.PersonType == 1)
                    {
                        var p = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                        PersonHesab = p.Id;
                        PersonSarfasl = p.SarfaslId.Value;
                    }
                    else if (PayModel.pay.PersonType == 2)
                    {
                        var p = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                        PersonHesab = p.Id;
                        PersonSarfasl = p.SarfaslId.Value;
                    }
                    else
                    {
                        var p = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                        PersonHesab = p.Id;
                        PersonSarfasl = p.SarfaslId.Value;
                    }
                    #region SectionEdit

                    if (PayModel.EditCash != null && PayModel.EditCash.Length > 0)
                    {
                        for (int i = 0; i < PayModel.EditCash.Length; i++)
                        {

                            if (!PayModel.deletedItem.Contain(PayModel.EditCash[i].id) && !PayModel.deletedItem.Contain(PayModel.EditCash[i].SecondId))
                            {
                                short cId = PayModel.EditCash[i].CashDeskId;
                                var CashHesab = _blTafsili.Where(m => m.CashDeskId == cId && m.SarfaslId == (short)SarfaslEnums.CashDesk).FirstOrDefault();
                                _blSanadItem.Update(PayModel.EditCash[i].id, sandId, FiscalId, date, null, null, PersonSarfasl, PersonHesab, PersonName.GetRecevieDescription(1), 0, PayModel.EditCash[i].Amount.ConvertToPrice(), false);
                                _blSanadItem.Update(PayModel.EditCash[i].SecondId, sandId, FiscalId, date, null, null, CashHesab.SarfaslId.Value, CashHesab.Id, PersonName.GetRecevieDescription(1), PayModel.EditCash[i].Amount.ConvertToPrice(), 0, false);
                                tempTotal += PayModel.EditCash[i].Amount.ConvertToPrice();
                            }
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.EditCard != null && PayModel.EditCard.Length > 0)
                    {
                        for (int i = 0; i < PayModel.EditCard.Length; i++)
                        {
                            if (!PayModel.deletedItem.Contain(PayModel.EditCard[i].id) && !PayModel.deletedItem.Contain(PayModel.EditCard[i].SecondId))
                            {
                                short BId = PayModel.EditCard[i].BankAccountId;
                                var BankHesab = _blTafsili.Where(m => m.BankAccId == BId && m.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                                _blSanadItem.Update(PayModel.EditCard[i].id, sandId, FiscalId, date, null, PayModel.EditCard[i].IssueTracking, PersonSarfasl, PersonHesab, PersonName.GetRecevieDescription(2, "", PayModel.EditCard[i].IssueTracking), 0, PayModel.EditCard[i].Amount.ConvertToPrice(), false);
                                _blSanadItem.Update(PayModel.EditCard[i].SecondId, sandId, FiscalId, date, null, PayModel.EditCard[i].IssueTracking, BankHesab.SarfaslId.Value, BankHesab.Id, PersonName.GetRecevieDescription(2, "", PayModel.EditCard[i].IssueTracking), PayModel.EditCard[i].Amount.ConvertToPrice(), 0, false);
                                tempTotal += PayModel.EditCard[i].Amount.ConvertToPrice();
                            }

                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.Editcheques != null && PayModel.Editcheques.Length > 0)
                    {

                        var tafsiliAsnadD = new Tafsili();
                        if (PayModel.pay.PersonType == 1)
                        {
                            tafsiliAsnadD = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.AsnadDaryftani).FirstOrDefault();

                        }
                        else if (PayModel.pay.PersonType == 2)
                        {
                            tafsiliAsnadD = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.AsnadDaryftani).FirstOrDefault();

                        }
                        else
                        {
                            tafsiliAsnadD = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.AsnadDaryftani).FirstOrDefault();

                        }

                        for (int i = 0; i < PayModel.Editcheques.Length; i++)
                        {
                            if (!PayModel.deteledCheque.Contain(PayModel.Editcheques[i].Id))
                            {

                                PayModel.Editcheques[i].StatusId = 1;
                                PayModel.Editcheques[i].DueDate = PayModel.Editcheques[i].DudateText.ToMiladiDate();
                                PayModel.Editcheques[i].Amount = PayModel.Editcheques[i].Price.ConvertToPrice();
                                _blcheque.Update(PayModel.Editcheques[i]);
                            }

                        }

                        for (int i = 0; i < PayModel.Editcheques.Length; i++)
                        {
                            if (!PayModel.deletedItem.Contain(PayModel.Editcheques[i].ChequeSanadId) && !PayModel.deletedItem.Contain(PayModel.Editcheques[i].ChequeSanadSecondId))
                            {
                                _blSanadItem.Update(PayModel.Editcheques[i].ChequeSanadId, sandId, FiscalId, date, PayModel.Editcheques[i].Id, null, PersonSarfasl, PersonHesab, PersonName.GetRecevieDescription(3, PayModel.Editcheques[i].ChequeNum, "", PayModel.Editcheques[i].DudateText), 0, PayModel.Editcheques[i].Price.ConvertToPrice(), false);
                                _blSanadItem.Update(PayModel.Editcheques[i].ChequeSanadSecondId, sandId, FiscalId, date, PayModel.Editcheques[i].Id, null, tafsiliAsnadD.SarfaslId.Value, tafsiliAsnadD.Id, PersonName.GetRecevieDescription(3, PayModel.Editcheques[i].ChequeNum, "", PayModel.Editcheques[i].DudateText), PayModel.Editcheques[i].Price.ConvertToPrice(), 0, false);
                                tempTotal += PayModel.Editcheques[i].Price.ConvertToPrice();
                            }

                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;
                        }
                        tempTotal = 0;

                    }

                    if (PayModel.deletedItem.Length > 0)
                    {
                        string[] ItemId = PayModel.deletedItem.Split(',');
                        for (int i = 0; i < ItemId.Length; i++)
                        {
                            if (ItemId[i] != "0")
                                _blSanadItem.Delete(int.Parse(ItemId[i]), false);
                        }

                        _blSanadItem.Save();
                    }

                    if (PayModel.deteledCheque.Length > 0)
                    {
                        string[] ItemId = PayModel.deteledCheque.Split(',');
                        for (int i = 0; i < ItemId.Length; i++)
                        {
                            if (ItemId[i] != "0")
                                _blcheque.Delete(int.Parse(ItemId[i]), out bool isUsed, false);
                        }

                        _blcheque.Save();
                    }
                    #endregion



                    #region NewSection
                    if (PayModel.cash != null && PayModel.cash.Length > 0)
                    {
                        for (int i = 0; i < PayModel.cash.Length; i++)
                        {
                            short cId = PayModel.cash[i].CashDeskId;
                            var CashHesab = _blTafsili.Where(m => m.CashDeskId == cId && m.SarfaslId == (short)SarfaslEnums.CashDesk).FirstOrDefault();
                            _blSanadItem.Add(sandId, FiscalId, date, null, null, PersonSarfasl, PersonHesab, PersonName.GetRecevieDescription(1), 0, PayModel.cash[i].Amount.ConvertToPrice(), false);
                            _blSanadItem.Add(sandId, FiscalId, date, null, null, CashHesab.SarfaslId.Value, CashHesab.Id, PersonName.GetRecevieDescription(1), PayModel.cash[i].Amount.ConvertToPrice(), 0, false);
                            tempTotal += PayModel.cash[i].Amount.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.Card != null && PayModel.Card.Length > 0)
                    {
                        for (int i = 0; i < PayModel.Card.Length; i++)
                        {
                            short BId = PayModel.Card[i].BankAccountId;
                            var BankHesab = _blTafsili.Where(m => m.BankAccId == BId && m.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                            _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, PersonSarfasl, PersonHesab, PersonName.GetRecevieDescription(2, "", PayModel.Card[i].IssueTracking), 0, PayModel.Card[i].Amount.ConvertToPrice(), false);
                            _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, BankHesab.SarfaslId.Value, BankHesab.Id, PersonName.GetRecevieDescription(2, "", PayModel.Card[i].IssueTracking), PayModel.Card[i].Amount.ConvertToPrice(), 0, false);
                            tempTotal += PayModel.Card[i].Amount.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.cheques != null && PayModel.cheques.Length > 0)
                    {

                        var tafsiliAsnadD = new Tafsili();
                        if (PayModel.pay.PersonType == 1)
                        {
                            tafsiliAsnadD = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.AsnadDaryftani).FirstOrDefault();

                        }
                        else if (PayModel.pay.PersonType == 2)
                        {
                            tafsiliAsnadD = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.AsnadDaryftani).FirstOrDefault();

                        }
                        else
                        {
                            tafsiliAsnadD = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.AsnadDaryftani).FirstOrDefault();

                        }

                        for (int i = 0; i < PayModel.cheques.Length; i++)
                        {

                            PayModel.cheques[i].StatusId = 1;
                            PayModel.cheques[i].DueDate = PayModel.cheques[i].DudateText.ToMiladiDate();
                            PayModel.cheques[i].Amount = PayModel.cheques[i].Price.ConvertToPrice();
                            _blcheque.Add(PayModel.cheques[i]);
                            int ChequeId = _blcheque.GetLastIdentity();
                            PayModel.cheques[i].Id = _blcheque.GetLastIdentity();
                        }

                        for (int i = 0; i < PayModel.cheques.Length; i++)
                        {
                            _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, PersonSarfasl, PersonHesab, PersonName.GetRecevieDescription(3, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), 0, PayModel.cheques[i].Price.ConvertToPrice(), false);
                            _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, tafsiliAsnadD.SarfaslId.Value, tafsiliAsnadD.Id, PersonName.GetRecevieDescription(3, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), PayModel.cheques[i].Price.ConvertToPrice(), 0, false);
                            tempTotal += PayModel.cheques[i].Price.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;
                        }
                        tempTotal = 0;

                    }

                    #endregion

                    if (Total != PayModel.pay.TotalAmount)
                    {
                        var sanad = _blSanad.Find(sandId, FiscalId);
                        sanad.TotalAmount = Total;
                        _blSanad.Update(sanad);
                    }

                    TempData["saveMassage"] = Notification.Show("ویرایش دریافت وجه / چک با موفقیت انجام شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return Json(new { RedirectUrl = Url.Action("Receives", "Transaction") });
                }
                else
                {
                    return Notification.jsonShow("ویرایش دریافت وجه / چک یا خطا رو به رو شد", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                return Notification.jsonShow(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);

            }

        }
        #endregion

        #region PaymentPart

        [HttpGet]
        [AccessControl(ActionsEnum.ShowTransactions)]
        public ActionResult Payments()
        {
            short fiscalId = Fiscal.GetFiscalId();
            return View(_blSanad.Where(i => i.SanadTypeId == (byte)SandType.Payment && i.PeriodId == fiscalId)
                .OrderByDescending(i => i.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddTransaction)]
        [FiscalCheck]
        public ActionResult AddPayment()
        {
            PaymentViewModel model = new PaymentViewModel();
            int code = (int)SarfaslGroup.hazineh;
            model.Sarfasls = _blSarfasl.Where(m => m.IsMoeen == true && m.Coding.StartsWith(code.ToString())).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Coding + "-" + s.Name }).OrderBy(m => m.Text);
            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.AddTransaction)]
        [FiscalCheck]
        public ActionResult AddPayment(PaymentViewModel PayModel)
        {
            if (ModelState.IsValid)
            {
                if (PayModel.cash == null && PayModel.Card == null && PayModel.cheques == null && string.IsNullOrEmpty(PayModel.PayChequeIds))
                {
                    return Notification.jsonShow("آیتمی برای این پرداخت وجه / چک وارد نشده است", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
                int PersonId = 0;
                string PersonName = "";
                string sarfaslName = "";
                bool personOrHesab = false;

                if (PayModel.pay.PersonType != 0 && PayModel.pay.Persons != "-1")
                {
                    personOrHesab = true;
                    PersonId = int.Parse(PayModel.pay.Persons.ToString().Split('/')[0]);
                    PersonName = PayModel.pay.Persons.ToString().Split('/')[1];
                    if (string.IsNullOrEmpty(PayModel.pay.Description))
                        PayModel.pay.Description = PersonName.GetPaymentDescription(4);
                }
                else if (PayModel.pay.sarfasl != 0)
                {
                    if (string.IsNullOrEmpty(PayModel.pay.Description))
                    {
                        var sarfasl = _blSarfasl.Find((short)PayModel.pay.sarfasl);
                        if (PayModel.pay.Tafsili.HasValue)
                        {
                            var Tafsili = _blTafsili.Find(PayModel.pay.Tafsili.Value);

                            sarfaslName = sarfasl.Name + " (" + Tafsili.OtherTafsili.Name + ") ";
                        }
                        else
                        {
                            sarfaslName = sarfasl.Name;
                        }
                        PayModel.pay.Description = sarfaslName.GetPaymentDescription(8);
                    }
                }
                short FiscalId = Fiscal.GetFiscalId();
                PayModel.pay.FiscalId = FiscalId;
                var date = DateTime.Now;
                PayModel.pay.SanadId = _blSanad.GetLastIdentity(FiscalId) + 1;
                if (_blSanad.AddPayment(PayModel.pay, date, UserIdentity.Getuserid()))
                {

                    int sandId = PayModel.pay.SanadId;
                    decimal Total = 0;
                    decimal tempTotal = 0;
                    int PersonHesab = 0;
                    short PersonSarfasl = 0;
                    if (personOrHesab)
                    {
                        if (PayModel.pay.PersonType == 1)
                        {
                            var p = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                            PersonHesab = p.Id;
                            PersonSarfasl = p.SarfaslId.Value;
                        }
                        else if (PayModel.pay.PersonType == 2)
                        {
                            var p = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                            PersonHesab = p.Id;
                            PersonSarfasl = p.SarfaslId.Value;
                        }
                        else
                        {
                            var p = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                            PersonHesab = p.Id;
                            PersonSarfasl = p.SarfaslId.Value;
                        }
                    }
                    if (PayModel.cash != null && PayModel.cash.Length > 0)
                    {
                        for (int i = 0; i < PayModel.cash.Length; i++)
                        {
                            short cId = PayModel.cash[i].CashDeskId;
                            var CashHesab = _blTafsili.Where(m => m.CashDeskId == cId && m.SarfaslId == (short)SarfaslEnums.CashDesk).FirstOrDefault();
                            if (personOrHesab)
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, null, null, PersonSarfasl, PersonHesab, PersonName.GetPaymentDescription(1), PayModel.cash[i].Amount.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, null, null, CashHesab.SarfaslId.Value, CashHesab.Id, PersonName.GetPaymentDescription(1), 0, PayModel.cash[i].Amount.ConvertToPrice(), false);
                            }

                            else
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, null, null, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(2), PayModel.cash[i].Amount.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, null, null, CashHesab.SarfaslId.Value, CashHesab.Id, sarfaslName.GetPaymentDescription(2), 0, PayModel.cash[i].Amount.ConvertToPrice(), false);
                            }
                            tempTotal += PayModel.cash[i].Amount.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.Card != null && PayModel.Card.Length > 0)
                    {
                        for (int i = 0; i < PayModel.Card.Length; i++)
                        {
                            short BId = PayModel.Card[i].BankAccountId;
                            var BankHesab = _blTafsili.Where(m => m.BankAccId == BId && m.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                            if (personOrHesab)
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, PersonSarfasl, PersonHesab, PersonName.GetPaymentDescription(3, "", PayModel.Card[i].IssueTracking), PayModel.Card[i].Amount.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, BankHesab.SarfaslId.Value, BankHesab.Id, PersonName.GetPaymentDescription(3, "", PayModel.Card[i].IssueTracking), 0, PayModel.Card[i].Amount.ConvertToPrice(), false);
                            }
                            else
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(4, "", PayModel.Card[i].IssueTracking), PayModel.Card[i].Amount.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, BankHesab.SarfaslId.Value, BankHesab.Id, sarfaslName.GetPaymentDescription(4, "", PayModel.Card[i].IssueTracking), 0, PayModel.Card[i].Amount.ConvertToPrice(), false);
                            }

                            tempTotal += PayModel.Card[i].Amount.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.cheques != null && PayModel.cheques.Length > 0)
                    {

                        var HesabPardakhtani = new Tafsili();
                        if (personOrHesab)
                        {
                            if (PayModel.pay.PersonType == 1)
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                            else if (PayModel.pay.PersonType == 2)
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                            else
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                        }

                        for (int i = 0; i < PayModel.cheques.Length; i++)
                        {

                            PayModel.cheques[i].StatusId = (byte)ChequeStatus.ChequePardakhti;
                            PayModel.cheques[i].DueDate = PayModel.cheques[i].DudateText.ToMiladiDate();
                            PayModel.cheques[i].Amount = PayModel.cheques[i].Price.ConvertToPrice();
                            _blcheque.Add(PayModel.cheques[i]);
                            int ChequeId = _blcheque.GetLastIdentity();
                            PayModel.cheques[i].Id = _blcheque.GetLastIdentity();
                        }

                        for (int i = 0; i < PayModel.cheques.Length; i++)
                        {
                            short CBId = PayModel.cheques[i].BankAccountId.Value;
                            var BankHesab = _blTafsili.Where(m => m.BankAccId == CBId && m.SarfaslId == (short)SarfaslEnums.AsnadPardakhati).FirstOrDefault();
                            if (personOrHesab)
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, HesabPardakhtani.SarfaslId.Value, HesabPardakhtani.Id, PersonName.GetPaymentDescription(5, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), PayModel.cheques[i].Price.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, BankHesab.SarfaslId.Value, BankHesab.Id, PersonName.GetPaymentDescription(5, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), 0, PayModel.cheques[i].Price.ConvertToPrice(), false);
                            }
                            else
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(6, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), PayModel.cheques[i].Price.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, BankHesab.SarfaslId.Value, BankHesab.Id, sarfaslName.GetPaymentDescription(6, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), 0, PayModel.cheques[i].Price.ConvertToPrice(), false);
                            }

                            tempTotal += PayModel.cheques[i].Price.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;
                        }
                        tempTotal = 0;


                    }

                    if (!string.IsNullOrEmpty(PayModel.PayChequeIds) && !string.IsNullOrWhiteSpace(PayModel.PayChequeIds))
                    {
                        string[] chequeId = PayModel.PayChequeIds.Split(',');
                        var HesabPardakhtani = new Tafsili();
                        if (personOrHesab)
                        {
                            if (PayModel.pay.PersonType == 1)
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                            else if (PayModel.pay.PersonType == 2)
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                            else
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                        }
                        Cheque[] cheque = new Cheque[chequeId.Length];
                        for (int i = 0; i < chequeId.Length; i++)
                        {
                            int Id = int.Parse(chequeId[i]);
                            cheque[i] = _blcheque.Find(Id);
                            cheque[i].Price = cheque[i].Amount.ToPrice();
                            cheque[i].DudateText = cheque[i].DueDate.ToPersianDateTime().ToString("yyyy/MM/dd");
                            cheque[i].StatusId = (byte)ChequeStatus.ChequeKharj;
                            _blcheque.Update(cheque[i]);
                            var T = _blSanadItem.Where(m => m.ChequeId == Id).ToArray()[1];
                            var per = T.Tafsili;
                            string Name = "";
                            if (per.CustomerId != null)
                                Name = per.Customer.OwnerName;
                            if (per.PersonnelId != null)
                                Name = per.Personnel.FullName;
                            if (per.SupplierId != null)
                                Name = per.Supplier.Name;
                            if (personOrHesab)
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, cheque[i].Id, null, HesabPardakhtani.SarfaslId.Value, HesabPardakhtani.Id, PersonName.GetPaymentDescription(9, cheque[i].ChequeNum, "", cheque[i].DudateText, Name), cheque[i].Price.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, cheque[i].Id, null, T.SarfaslId, T.TafsiliId, PersonName.GetPaymentDescription(9, cheque[i].ChequeNum, "", cheque[i].DudateText, Name), 0, cheque[i].Price.ConvertToPrice(), false);
                            }
                            else
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, cheque[i].Id, null, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(10, cheque[i].ChequeNum, "", cheque[i].DudateText, Name), cheque[i].Price.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, cheque[i].Id, null, T.SarfaslId, T.TafsiliId, sarfaslName.GetPaymentDescription(10, cheque[i].ChequeNum, "", cheque[i].DudateText, Name), 0, cheque[i].Price.ConvertToPrice(), false);
                            }
                            tempTotal += cheque[i].Price.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;
                        }
                        tempTotal = 0;
                    }
                    if (Total != PayModel.pay.TotalAmount)
                    {
                        var sanad = _blSanad.Find(sandId, FiscalId);
                        sanad.TotalAmount = Total;
                        _blSanad.Update(sanad);
                    }

                    TempData["saveMassage"] = Notification.Show("ثبت پرداخت وجه / چک با موفقیت انجام شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return Json(new { RedirectUrl = Url.Action("Payments", "Transaction") });
                }
                else
                {
                    return Notification.jsonShow("ثبت پرداخت وجه / چک یا خطا رو به رو شد", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                return Notification.jsonShow(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateTransaction)]
        [FiscalCheck]
        [SanadUpdateCheck]
        public ActionResult UpdatePayment(int? PaymentId)
        {
            if (PaymentId.HasValue)
            {
                var model = new PaymentViewModel();
                model.CashDesks = _blCashDesk.Where(m => m.IsAvailable == true);
                model.BankAccount = _blBank.SelectView();
                model.ChequeBankAccount = _blBank.SelectView();
                var pay = new PaymentModel();
                model.deletedItem = "0";
                model.deteledCheque = "0";
                model.deteledPayCheque = "0";
                short FiscalId = Fiscal.GetFiscalId();
                var sanad = _blSanad.Find(PaymentId.Value, FiscalId);
                var sItems = sanad.SanadItems;
                var Cash = sItems.Where(m => m.IssueTrackingNum == null && m.ChequeId == null).ToArray();
                var Card = sItems.Where(m => m.IssueTrackingNum != null).ToArray();
                var Cheque = sItems.Where(m => m.ChequeId != null && m.Cheque.StatusId != 6).ToArray();
                var PayCheque = sItems.Where(m => m.ChequeId != null && m.Cheque.StatusId == 6).ToArray();
                var sanadItem = sItems.FirstOrDefault();
                int PersonTypeId = 0;
                string personName = "";
                string HesabName = "";
                string TafsiliName = "";
                if (sanadItem.TafsiliId != null)
                {
                    var Tafsili = sanadItem.Tafsili;
                    decimal bes = 0;
                    decimal bed = 0;
                    if (Tafsili.PersonTypeId != null)
                    {
                        PersonTypeId = (short)Tafsili.PersonTypeId;
                        switch (PersonTypeId)
                        {
                            case 1:
                                var personnel = Tafsili.Personnel;
                                personName = personnel.Id + "/" + personnel.FullName;
                                var PersonnelD = _blTafsili.Where(m => m.PersonnelId == personnel.Id && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                                var PersonnelP = _blTafsili.Where(m => m.PersonnelId == personnel.Id && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                                var bestanp = _blSanadItem.Where(m => (m.TafsiliId == PersonnelD.Id || m.TafsiliId == PersonnelP.Id) && m.SandId != PaymentId);
                                var bedehp = _blSanadItem.Where(m => (m.TafsiliId == PersonnelD.Id || m.TafsiliId == PersonnelP.Id) && m.SandId != PaymentId);
                                if (bestanp.Any())
                                    bes = bestanp.Sum(m => m.Bestan);
                                else
                                    bes = 0;

                                if (bedehp.Any())
                                    bed = bedehp.Sum(m => m.Bedeh);
                                else
                                    bes = 0;

                                break;
                            case 2:
                                var customer = Tafsili.Customer;
                                personName = customer.Id + "/" + customer.OwnerName;
                                var CustomerD = _blTafsili.Where(m => m.CustomerId == customer.Id && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                                var CustomerP = _blTafsili.Where(m => m.CustomerId == customer.Id && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                                var bestanc = _blSanadItem.Where(m => (m.TafsiliId == CustomerD.Id || m.TafsiliId == CustomerP.Id) && m.SandId != PaymentId);
                                var bedehc = _blSanadItem.Where(m => (m.TafsiliId == CustomerD.Id || m.TafsiliId == CustomerP.Id) && m.SandId != PaymentId);
                                if (bestanc.Any())
                                    bes = bestanc.Sum(m => m.Bestan);
                                else
                                    bes = 0;

                                if (bedehc.Any())
                                    bed = bedehc.Sum(m => m.Bedeh);
                                else
                                    bes = 0;
                                break;
                            case 3:
                                var supplier = Tafsili.Supplier;
                                personName = supplier.Id + "/" + supplier.Name;
                                var SupplierD = _blTafsili.Where(m => m.SupplierId == supplier.Id && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                                var SupplierP = _blTafsili.Where(m => m.SupplierId == supplier.Id && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                                var bestans = _blSanadItem.Where(m => (m.TafsiliId == SupplierD.Id || m.TafsiliId == SupplierP.Id) && m.SandId != PaymentId);
                                var bedehs = _blSanadItem.Where(m => (m.TafsiliId == SupplierD.Id || m.TafsiliId == SupplierP.Id) && m.SandId != PaymentId);
                                if (bestans.Any())
                                    bes = bestans.Sum(m => m.Bestan);
                                else
                                    bes = 0;

                                if (bedehs.Any())
                                    bed = bedehs.Sum(m => m.Bedeh);
                                else
                                    bes = 0;
                                break;
                        };
                        model.Taraz = bes - bed;
                    }
                    else
                    {

                        HesabName = sanadItem.SarfaslId + "/" + sanadItem.Sarfasl.Name;
                        TafsiliName = Tafsili.Id + "/" + Tafsili.OtherTafsili.Name;
                    }
                }
                else
                {
                    HesabName = sanadItem.SarfaslId + "/" + sanadItem.Sarfasl.Name;
                }
                #region sanadPart
                pay.PersonType = PersonTypeId;
                pay.Persons = personName;
                pay.sarfaslText = HesabName;
                pay.TafsiliText = TafsiliName;
                pay.Description = sanad.Description;
                pay.TotalAmount = sanad.TotalAmount;
                pay.SanadId = PaymentId.Value;
                model.pay = pay;
                #endregion
                #region CashPart
                int CashCount = Cash.Count();
                if (CashCount > 0)
                {
                    CashModel[] cashes = new CashModel[CashCount / 2];
                    for (int i = 0; i < CashCount; i++)
                    {
                        int m = 0;
                        if ((i % 2) == 0)
                        {
                            if (i != 0)
                                m = i - 1;
                            CashModel cm = new CashModel();
                            cm.id = Cash[i].Id;
                            cm.SecondId = Cash[i + 1].Id;
                            cm.CashDeskId = Cash[i + 1].Tafsili.CashDeskId.Value;
                            cm.Amount = Cash[i].Bedeh.ToPrice();
                            cashes[m] = cm;

                        }

                    }
                    model.EditCash = cashes;
                }
                #endregion
                #region CardPart
                int CardCount = Card.Count();
                if (CardCount > 0)
                {
                    CardModel[] Cards = new CardModel[CardCount / 2];
                    for (int i = 0; i < CardCount; i++)
                    {
                        int m = 0;
                        if ((i % 2) == 0)
                        {
                            if (i != 0)
                                m = i - 1;
                            CardModel cm = new CardModel();
                            cm.id = Card[i].Id;
                            cm.SecondId = Card[i + 1].Id;
                            var t = Card[i + 1].Tafsili;
                            cm.BankAccountId = t.BankAccId.Value;
                            var b = t.BankAccount;
                            cm.CardAccountNumber = b.AccountNum + "/" + b.Owner;
                            cm.Amount = Card[i].Bedeh.ToPrice();
                            cm.IssueTracking = Card[i].IssueTrackingNum;
                            Cards[m] = cm;

                        }

                    }
                    model.EditCard = Cards;
                }
                #endregion
                #region ChequePart
                int ChequeCount = Cheque.Count();
                if (ChequeCount > 0)
                {
                    Cheque[] cheques = new Cheque[ChequeCount / 2];
                    for (int i = 0; i < ChequeCount; i++)
                    {
                        int m = 0;
                        if ((i % 2) == 0)
                        {
                            if (i != 0)
                                m = i - 1;
                            Cheque cm = new Cheque();
                            var ch = Cheque[i].Cheque;
                            cm.Id = ch.Id;
                            cm.ChequeNum = ch.ChequeNum;
                            cm.Price = ch.Amount.ToPrice();
                            cm.DudateText = ch.DueDate.ToPersianDateTime().ToString("yyyy/MM/dd");
                            cm.BankAccountId = ch.BankAccountId;

                            cm.Description = ch.Description;
                            cm.ChequeSanadId = Cheque[i].Id;
                            cm.ChequeSanadSecondId = Cheque[i + 1].Id;
                            cheques[m] = cm;

                        }

                    }
                    model.Editcheques = cheques;
                }
                #endregion
                #region PayChequePart
                int PayChequeCount = PayCheque.Count();
                if (PayChequeCount > 0)
                {
                    Cheque[] cheques = new Cheque[PayChequeCount / 2];
                    for (int i = 0; i < PayChequeCount; i++)
                    {
                        int m = 0;
                        if ((i % 2) == 0)
                        {
                            if (i != 0)
                                m = i - 1;
                            Cheque cm = new Cheque();
                            var ch = PayCheque[i].Cheque;
                            cm.Id = ch.Id;
                            cm.ChequeNum = ch.ChequeNum;
                            cm.Price = ch.Amount.ToPrice();
                            cm.Amount = ch.Amount;
                            cm.DudateText = ch.DueDate.ToPersianDateTime().ToString("yyyy/MM/dd");
                            cm.BankId = ch.BankId;
                            cm.Description = ch.Description;
                            cm.ChequeSanadId = PayCheque[i].Id;
                            cm.ChequeSanadSecondId = PayCheque[i + 1].Id;
                            cm.Bank = ch.Bank;
                            cheques[m] = cm;

                        }

                    }
                    model.EditPaycheques = cheques;
                }
                #endregion
                return View(model);
            }
            else
            {
                //return To Not Found
            }
            return View();
        }

        [HttpPost]
        [AccessControl(ActionsEnum.UpdateTransaction)]
        [FiscalCheck]

        public ActionResult UpdatePayment(PaymentViewModel PayModel)
        {
            if (ModelState.IsValid)
            {
                if (PayModel.pay.TotalAmount == 0)
                {
                    return Notification.jsonShow("آیتمی برای ویرایش در این دریافت وجه / چک وجود ندارد", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
                int PersonId = 0;
                string PersonName = "";
                string sarfaslName = "";
                bool personOrHesab = false;
                if (PayModel.pay.PersonType != 0 && PayModel.pay.Persons != "-1")
                {
                    personOrHesab = true;
                    PersonId = int.Parse(PayModel.pay.Persons.ToString().Split('/')[0]);
                    PersonName = PayModel.pay.Persons.ToString().Split('/')[1];
                    if (string.IsNullOrEmpty(PayModel.pay.Description))
                        PayModel.pay.Description = PersonName.GetPaymentDescription(7);
                }
                else if (PayModel.pay.sarfasl != 0)
                {
                    var sarfasl = _blSarfasl.Find((short)PayModel.pay.sarfasl);
                    if (string.IsNullOrEmpty(PayModel.pay.Description))
                    {

                        if (PayModel.pay.Tafsili.HasValue)
                        {
                            var Tafsili = _blTafsili.Find(PayModel.pay.Tafsili.Value);

                            sarfaslName = sarfasl.Name + " (" + Tafsili.OtherTafsili.Name + ") ";
                        }
                        else
                        {
                            sarfaslName = sarfasl.Name;
                        }
                        PayModel.pay.Description = sarfaslName.GetPaymentDescription(8);
                    }
                    else
                    {
                        sarfaslName = sarfasl.Name;
                    }
                }
                short FiscalId = Fiscal.GetFiscalId();
                PayModel.pay.FiscalId = FiscalId;
                var date = new DateTime();
                if (_blSanad.UpdatePayment(PayModel.pay, ref date))
                {
                    int sandId = PayModel.pay.SanadId;
                    decimal Total = 0;
                    decimal tempTotal = 0;
                    int PersonHesab = 0;
                    short PersonSarfasl = 0;
                    if (personOrHesab)
                    {
                        if (PayModel.pay.PersonType == 1)
                        {
                            var p = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                            PersonHesab = p.Id;
                            PersonSarfasl = p.SarfaslId.Value;
                        }
                        else if (PayModel.pay.PersonType == 2)
                        {
                            var p = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                            PersonHesab = p.Id;
                            PersonSarfasl = p.SarfaslId.Value;
                        }
                        else
                        {
                            var p = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                            PersonHesab = p.Id;
                            PersonSarfasl = p.SarfaslId.Value;
                        }
                    }
                    #region SectionEdit

                    if (PayModel.EditCash != null && PayModel.EditCash.Length > 0)
                    {
                        for (int i = 0; i < PayModel.EditCash.Length; i++)
                        {

                            if (!PayModel.deletedItem.Contain(PayModel.EditCash[i].id) && !PayModel.deletedItem.Contain(PayModel.EditCash[i].SecondId))
                            {
                                short cId = PayModel.EditCash[i].CashDeskId;
                                var CashHesab = _blTafsili.Where(m => m.CashDeskId == cId && m.SarfaslId == (short)SarfaslEnums.CashDesk).FirstOrDefault();
                                if (personOrHesab)
                                {
                                    _blSanadItem.Update(PayModel.EditCash[i].id, sandId, FiscalId, date, null, null, PersonSarfasl, PersonHesab, PersonName.GetPaymentDescription(1), PayModel.EditCash[i].Amount.ConvertToPrice(), 0, false);
                                    _blSanadItem.Update(PayModel.EditCash[i].SecondId, sandId, FiscalId, date, null, null, CashHesab.SarfaslId.Value, CashHesab.Id, PersonName.GetPaymentDescription(1), 0, PayModel.EditCash[i].Amount.ConvertToPrice(), false);
                                }

                                else
                                {
                                    _blSanadItem.Update(PayModel.EditCash[i].id, sandId, FiscalId, date, null, null, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(2), PayModel.EditCash[i].Amount.ConvertToPrice(), 0, false);
                                    _blSanadItem.Update(PayModel.EditCash[i].SecondId, sandId, FiscalId, date, null, null, CashHesab.SarfaslId.Value, CashHesab.Id, sarfaslName.GetPaymentDescription(2), 0, PayModel.EditCash[i].Amount.ConvertToPrice(), false);
                                }
                                tempTotal += PayModel.EditCash[i].Amount.ConvertToPrice();
                            }
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.EditCard != null && PayModel.EditCard.Length > 0)
                    {
                        for (int i = 0; i < PayModel.EditCard.Length; i++)
                        {
                            if (!PayModel.deletedItem.Contain(PayModel.EditCard[i].id) && !PayModel.deletedItem.Contain(PayModel.EditCard[i].SecondId))
                            {
                                short BId = PayModel.EditCard[i].BankAccountId;
                                var BankHesab = _blTafsili.Where(m => m.BankAccId == BId && m.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                                if (personOrHesab)
                                {
                                    _blSanadItem.Update(PayModel.EditCard[i].id, sandId, FiscalId, date, null, PayModel.EditCard[i].IssueTracking, PersonSarfasl, PersonHesab, PersonName.GetPaymentDescription(3, "", PayModel.EditCard[i].IssueTracking), PayModel.EditCard[i].Amount.ConvertToPrice(), 0, false);
                                    _blSanadItem.Update(PayModel.EditCard[i].SecondId, sandId, FiscalId, date, null, PayModel.EditCard[i].IssueTracking, BankHesab.SarfaslId.Value, BankHesab.Id, PersonName.GetPaymentDescription(3, "", PayModel.EditCard[i].IssueTracking), 0, PayModel.EditCard[i].Amount.ConvertToPrice(), false);
                                }
                                else
                                {
                                    _blSanadItem.Update(PayModel.EditCard[i].id, sandId, FiscalId, date, null, PayModel.EditCard[i].IssueTracking, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(4, "", PayModel.EditCard[i].IssueTracking), PayModel.EditCard[i].Amount.ConvertToPrice(), 0, false);
                                    _blSanadItem.Update(PayModel.EditCard[i].SecondId, sandId, FiscalId, date, null, PayModel.EditCard[i].IssueTracking, BankHesab.SarfaslId.Value, BankHesab.Id, sarfaslName.GetPaymentDescription(4, "", PayModel.EditCard[i].IssueTracking), 0, PayModel.EditCard[i].Amount.ConvertToPrice(), false);
                                }

                                tempTotal += PayModel.EditCard[i].Amount.ConvertToPrice();
                            }

                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.Editcheques != null && PayModel.Editcheques.Length > 0)
                    {

                        var HesabPardakhtani = new Tafsili();
                        if (personOrHesab)
                        {
                            if (PayModel.pay.PersonType == 1)
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                            else if (PayModel.pay.PersonType == 2)
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                            else
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                        }

                        for (int i = 0; i < PayModel.Editcheques.Length; i++)
                        {
                            if (!PayModel.deteledCheque.Contain(PayModel.Editcheques[i].Id))
                            {

                                PayModel.Editcheques[i].StatusId = (byte)ChequeStatus.ChequePardakhti;
                                PayModel.Editcheques[i].DueDate = PayModel.Editcheques[i].DudateText.ToMiladiDate();
                                PayModel.Editcheques[i].Amount = PayModel.Editcheques[i].Price.ConvertToPrice();
                                _blcheque.Update(PayModel.Editcheques[i]);
                            }

                        }

                        for (int i = 0; i < PayModel.Editcheques.Length; i++)
                        {
                            if (!PayModel.deteledCheque.Contain(PayModel.Editcheques[i].Id))
                            {
                                short CBId = PayModel.Editcheques[i].BankAccountId.Value;
                                var BankHesab = _blTafsili.Where(m => m.BankAccId == CBId && m.SarfaslId == (short)SarfaslEnums.AsnadPardakhati).FirstOrDefault();
                                if (personOrHesab)
                                {
                                    _blSanadItem.Update(PayModel.Editcheques[i].ChequeSanadId, sandId, FiscalId, date, PayModel.Editcheques[i].Id, null, HesabPardakhtani.SarfaslId.Value, HesabPardakhtani.Id, PersonName.GetPaymentDescription(5, PayModel.Editcheques[i].ChequeNum, "", PayModel.Editcheques[i].DudateText), PayModel.Editcheques[i].Price.ConvertToPrice(), 0, false);
                                    _blSanadItem.Update(PayModel.Editcheques[i].ChequeSanadSecondId, sandId, FiscalId, date, PayModel.Editcheques[i].Id, null, BankHesab.SarfaslId.Value, BankHesab.Id, PersonName.GetPaymentDescription(5, PayModel.Editcheques[i].ChequeNum, "", PayModel.Editcheques[i].DudateText), 0, PayModel.Editcheques[i].Price.ConvertToPrice(), false);
                                }
                                else
                                {
                                    _blSanadItem.Update(PayModel.Editcheques[i].ChequeSanadId, sandId, FiscalId, date, PayModel.Editcheques[i].Id, null, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(6, PayModel.Editcheques[i].ChequeNum, "", PayModel.Editcheques[i].DudateText), PayModel.Editcheques[i].Price.ConvertToPrice(), 0, false);
                                    _blSanadItem.Update(PayModel.Editcheques[i].ChequeSanadSecondId, sandId, FiscalId, date, PayModel.Editcheques[i].Id, null, BankHesab.SarfaslId.Value, BankHesab.Id, sarfaslName.GetPaymentDescription(6, PayModel.Editcheques[i].ChequeNum, "", PayModel.Editcheques[i].DudateText), 0, PayModel.Editcheques[i].Price.ConvertToPrice(), false);
                                }
                                tempTotal += PayModel.Editcheques[i].Price.ConvertToPrice();
                            }
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;
                        }
                        tempTotal = 0;


                    }

                    if (PayModel.EditPaycheques != null && PayModel.EditPaycheques.Length > 0)
                    {
                        for (int i = 0; i < PayModel.EditPaycheques.Length; i++)
                        {
                            if (!PayModel.deteledPayCheque.Contain(PayModel.EditPaycheques[i].Id))
                            {
                                tempTotal += PayModel.EditPaycheques[i].Price.ConvertToPrice();
                            }

                        }
                        Total += tempTotal;
                        tempTotal = 0;
                    }

                    if (!string.IsNullOrEmpty(PayModel.deletedItem) && !string.IsNullOrWhiteSpace(PayModel.deletedItem))
                    {

                        string[] ItemId = PayModel.deletedItem.Split(',');
                        for (int i = 0; i < ItemId.Length; i++)
                        {
                            if (ItemId[i] != "0")
                                _blSanadItem.Delete(int.Parse(ItemId[i]), false);
                        }
                        _blSanadItem.Save();
                    }

                    if (!string.IsNullOrEmpty(PayModel.deteledCheque) && !string.IsNullOrWhiteSpace(PayModel.deteledCheque))
                    {

                        string[] ItemId = PayModel.deteledCheque.Split(',');
                        for (int i = 0; i < ItemId.Length; i++)
                        {
                            if (ItemId[i] != "0")
                                _blcheque.Delete(int.Parse(ItemId[i]), out bool isUsed, false);
                        }

                        _blcheque.Save();
                    }

                    if (!string.IsNullOrEmpty(PayModel.deteledPayCheque) && !string.IsNullOrWhiteSpace(PayModel.deteledPayCheque))
                    {
                        string[] ItemId = PayModel.deteledPayCheque.Split(',');
                        for (int i = 0; i < ItemId.Length; i++)
                        {
                            if (ItemId[i] != "0")
                            {
                                int Id = int.Parse(ItemId[i]);
                                var cheque = _blcheque.Find(Id);
                                cheque.Price = cheque.Amount.ToPrice();
                                cheque.DudateText = cheque.DueDate.ToPersianDateTime().ToString("yyyy/MM/dd");
                                cheque.StatusId = (byte)ChequeStatus.chequeDaryafti;
                                _blcheque.Update(cheque);
                            }


                        }

                        _blcheque.Save();
                    }
                    #endregion

                    #region NewSection

                    if (PayModel.cash != null && PayModel.cash.Length > 0)
                    {
                        for (int i = 0; i < PayModel.cash.Length; i++)
                        {
                            short cId = PayModel.cash[i].CashDeskId;
                            var CashHesab = _blTafsili.Where(m => m.CashDeskId == cId && m.SarfaslId == (short)SarfaslEnums.CashDesk).FirstOrDefault();
                            if (personOrHesab)
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, null, null, PersonSarfasl, PersonHesab, PersonName.GetPaymentDescription(1), PayModel.cash[i].Amount.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, null, null, CashHesab.SarfaslId.Value, CashHesab.Id, PersonName.GetRecevieDescription(1), 0, PayModel.cash[i].Amount.ConvertToPrice(), false);
                            }

                            else
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, null, null, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(2), PayModel.cash[i].Amount.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, null, null, CashHesab.SarfaslId.Value, CashHesab.Id, sarfaslName.GetPaymentDescription(1), 0, PayModel.cash[i].Amount.ConvertToPrice(), false);
                            }
                            tempTotal += PayModel.cash[i].Amount.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.Card != null && PayModel.Card.Length > 0)
                    {
                        for (int i = 0; i < PayModel.Card.Length; i++)
                        {
                            short BId = PayModel.Card[i].BankAccountId;
                            var BankHesab = _blTafsili.Where(m => m.BankAccId == BId && m.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                            if (personOrHesab)
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, PersonSarfasl, PersonHesab, PersonName.GetPaymentDescription(3, "", PayModel.Card[i].IssueTracking), PayModel.Card[i].Amount.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, BankHesab.SarfaslId.Value, BankHesab.Id, PersonName.GetPaymentDescription(3, "", PayModel.Card[i].IssueTracking), 0, PayModel.Card[i].Amount.ConvertToPrice(), false);
                            }
                            else
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(4, "", PayModel.Card[i].IssueTracking), PayModel.Card[i].Amount.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, null, PayModel.Card[i].IssueTracking, BankHesab.SarfaslId.Value, BankHesab.Id, sarfaslName.GetPaymentDescription(4, "", PayModel.Card[i].IssueTracking), 0, PayModel.Card[i].Amount.ConvertToPrice(), false);
                            }

                            tempTotal += PayModel.Card[i].Amount.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;

                        }
                        tempTotal = 0;
                    }

                    if (PayModel.cheques != null && PayModel.cheques.Length > 0)
                    {

                        var HesabPardakhtani = new Tafsili();
                        if (personOrHesab)
                        {
                            if (PayModel.pay.PersonType == 1)
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                            else if (PayModel.pay.PersonType == 2)
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                            else
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                        }

                        for (int i = 0; i < PayModel.cheques.Length; i++)
                        {

                            PayModel.cheques[i].StatusId = (byte)ChequeStatus.ChequePardakhti;
                            PayModel.cheques[i].DueDate = PayModel.cheques[i].DudateText.ToMiladiDate();
                            PayModel.cheques[i].Amount = PayModel.cheques[i].Price.ConvertToPrice();
                            _blcheque.Add(PayModel.cheques[i]);
                            int ChequeId = _blcheque.GetLastIdentity();
                            PayModel.cheques[i].Id = _blcheque.GetLastIdentity();
                        }

                        for (int i = 0; i < PayModel.cheques.Length; i++)
                        {
                            short CBId = PayModel.cheques[i].BankAccountId.Value;
                            var BankHesab = _blTafsili.Where(m => m.BankAccId == CBId && m.SarfaslId == (short)SarfaslEnums.AsnadPardakhati).FirstOrDefault();
                            if (personOrHesab)
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, HesabPardakhtani.SarfaslId.Value, HesabPardakhtani.Id, PersonName.GetPaymentDescription(5, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), PayModel.cheques[i].Price.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, BankHesab.SarfaslId.Value, BankHesab.Id, PersonName.GetPaymentDescription(5, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), 0, PayModel.cheques[i].Price.ConvertToPrice(), false);
                            }
                            else
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(6, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), PayModel.cheques[i].Price.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, PayModel.cheques[i].Id, null, BankHesab.SarfaslId.Value, BankHesab.Id, sarfaslName.GetPaymentDescription(6, PayModel.cheques[i].ChequeNum, "", PayModel.cheques[i].DudateText), 0, PayModel.cheques[i].Price.ConvertToPrice(), false);
                            }

                            tempTotal += PayModel.cheques[i].Price.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;
                        }
                        tempTotal = 0;


                    }

                    if (!string.IsNullOrEmpty(PayModel.PayChequeIds) && !string.IsNullOrWhiteSpace(PayModel.PayChequeIds))
                    {
                        string[] chequeId = PayModel.PayChequeIds.Split(',');
                        var HesabPardakhtani = new Tafsili();
                        if (personOrHesab)
                        {
                            if (PayModel.pay.PersonType == 1)
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                            else if (PayModel.pay.PersonType == 2)
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                            else
                            {
                                HesabPardakhtani = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();

                            }
                        }
                        Cheque[] cheque = new Cheque[chequeId.Length];
                        for (int i = 0; i < chequeId.Length; i++)
                        {
                            int Id = int.Parse(chequeId[i]);
                            cheque[i] = _blcheque.Find(Id);
                            cheque[i].Price = cheque[i].Amount.ToPrice();
                            cheque[i].DudateText = cheque[i].DueDate.ToPersianDateTime().ToString("yyyy/MM/dd");
                            cheque[i].StatusId = (byte)ChequeStatus.ChequeKharj;
                            _blcheque.Update(cheque[i]);
                            var T = _blSanadItem.Where(m => m.ChequeId == Id).ToArray()[1];
                            var per = T.Tafsili;
                            string Name = "";
                            if (per.CustomerId != null)
                                Name = per.Customer.OwnerName;
                            if (per.PersonnelId != null)
                                Name = per.Personnel.FullName;
                            if (per.SupplierId != null)
                                Name = per.Supplier.Name;
                            if (personOrHesab)
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, cheque[i].Id, null, HesabPardakhtani.SarfaslId.Value, HesabPardakhtani.Id, PersonName.GetPaymentDescription(9, cheque[i].ChequeNum, "", cheque[i].DudateText, Name), cheque[i].Price.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, cheque[i].Id, null, T.SarfaslId, T.TafsiliId, PersonName.GetPaymentDescription(9, cheque[i].ChequeNum, "", cheque[i].DudateText, Name), 0, cheque[i].Price.ConvertToPrice(), false);
                            }
                            else
                            {
                                _blSanadItem.Add(sandId, FiscalId, date, cheque[i].Id, null, (short)PayModel.pay.sarfasl, PayModel.pay.Tafsili, sarfaslName.GetPaymentDescription(10, cheque[i].ChequeNum, "", cheque[i].DudateText, Name), cheque[i].Price.ConvertToPrice(), 0, false);
                                _blSanadItem.Add(sandId, FiscalId, date, cheque[i].Id, null, T.SarfaslId, T.TafsiliId, sarfaslName.GetPaymentDescription(10, cheque[i].ChequeNum, "", cheque[i].DudateText, Name), 0, cheque[i].Price.ConvertToPrice(), false);
                            }
                            tempTotal += cheque[i].Price.ConvertToPrice();
                        }
                        if (_blSanadItem.Save())
                        {
                            Total += tempTotal;
                        }
                        tempTotal = 0;
                    }

                    if (Total != PayModel.pay.TotalAmount)
                    {
                        var sanad = _blSanad.Find(sandId, FiscalId);
                        sanad.TotalAmount = Total;
                        _blSanad.Update(sanad);
                    }
                    #endregion
                    TempData["saveMassage"] = Notification.Show("ویرایش پرداخت وجه / چک با موفقیت انجام شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return Json(new { RedirectUrl = Url.Action("Payments", "Transaction", new { PaymentId = sandId }) });
                }
                else
                {
                    return Notification.jsonShow("ویرایش پرداخت وجه / چک با خطا رو به رو شد", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                return Notification.jsonShow(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);

            }

        }

        #endregion

        #region Transfer
        [HttpGet]
        [AccessControl(ActionsEnum.ShowTransactions)]
        public ActionResult Transfers()
        {
            short periodId = Fiscal.GetFiscalId();
            return View(_blSanad.Where(i => i.SanadTypeId == (byte)SandType.EntegalVajh && i.PeriodId == periodId)
                .OrderByDescending(i => i.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddTransaction)]
        [FiscalCheck]
        public ActionResult AddTransfer()
        {
            var model = new TransferViewModel();
            model.cashDesks = _blCashDesk.Select();
            model.Banks = _blBank.SelectView();
            model.Date = DateTime.Now.ToPersianDateTime().ToString("yyyy/MM/dd");
            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.AddTransaction)]
        [FiscalCheck]
        public ActionResult AddTransfer(TransferViewModel transfer)
        {
            if (ModelState.IsValid)
            {
                if ((transfer.FromBank != 0 || transfer.FromCashDesk != 0) && (transfer.ToBank != 0 || transfer.ToCashDesk != 0))
                {
                    short FromId = transfer.FromBank == 0 ? transfer.FromCashDesk : transfer.FromBank;
                    short ToId = transfer.ToBank == 0 ? transfer.ToCashDesk : transfer.ToBank;

                    if (transfer.FromBank != 0 && transfer.ToBank != 0 && FromId == ToId)
                    {
                        return Notification.jsonShow("مبدا و مقصد انتقال باید متفاوت باشد", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    }

                    if (transfer.FromCashDesk != 0 && transfer.ToCashDesk != 0 && FromId == ToId)
                    {
                        return Notification.jsonShow("مبدا و مقصد انتقال باید متفاوت باشد", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    }
                    string FromName = "";
                    string ToName = "";
                    short FromSarfasl = 0;
                    int FromTafsili = 0;
                    short ToSarfasl = 0;
                    int ToTafsili = 0;
                    if (transfer.FromBank != 0)
                    {
                        FromName = _blBank.Find(FromId).Bank.Name;
                        var Tafsili = _blTafsili.Where(m => m.BankAccId == FromId && m.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                        FromSarfasl = (short)SarfaslEnums.Bank;
                        FromTafsili = Tafsili.Id;
                    }
                    if (transfer.FromCashDesk != 0)
                    {
                        FromName = _blCashDesk.Find(FromId).Name;
                        var Tafsili = _blTafsili.Where(m => m.CashDeskId == FromId && m.SarfaslId == (short)SarfaslEnums.CashDesk).FirstOrDefault();
                        FromSarfasl = (short)SarfaslEnums.CashDesk;
                        FromTafsili = Tafsili.Id;
                    }
                    if (transfer.ToBank != 0)
                    {
                        ToName = _blBank.Find(ToId).Bank.Name;
                        var Tafsili = _blTafsili.Where(m => m.BankAccId == ToId && m.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                        ToSarfasl = (short)SarfaslEnums.Bank;
                        ToTafsili = Tafsili.Id;
                    }
                    if (transfer.ToCashDesk != 0)
                    {
                        ToName = _blCashDesk.Find(ToId).Name;
                        var Tafsili = _blTafsili.Where(m => m.CashDeskId == ToId && m.SarfaslId == (short)SarfaslEnums.CashDesk).FirstOrDefault();
                        ToSarfasl = (short)SarfaslEnums.CashDesk;
                        ToTafsili = Tafsili.Id;
                    }
                    Sanad sanad = new Sanad();
                    sanad.CreatedDate = DateTime.Now;
                    sanad.ConfirmDate = transfer.Date.ToMiladiDate();
                    sanad.SanadTypeId = (byte)SandType.EntegalVajh;
                    sanad.IsAutomatic = true;
                    sanad.IsConfirmed = true;
                    sanad.CreatedByUserId = UserIdentity.Getuserid();
                    sanad.TotalAmount = transfer.Amount.ConvertToPrice();
                    if (string.IsNullOrEmpty(transfer.Description))
                        sanad.Description = FromName.GetTransferDescription(ToName, transfer.FromIssueTracking, transfer.ToIssueTracking);
                    else
                        sanad.Description = transfer.Description;
                    short FiscalId = Fiscal.GetFiscalId();
                    sanad.PeriodId = FiscalId;
                    sanad.Id = _blSanad.GetLastIdentity(FiscalId) + 1;
                    if (_blSanad.Add(sanad))
                    {
                        int id = sanad.Id;
                        _blSanadItem.Add(id, FiscalId, sanad.ConfirmDate, null, transfer.FromIssueTracking, FromSarfasl, FromTafsili, sanad.Description, 0, sanad.TotalAmount, false);
                        _blSanadItem.Add(id, FiscalId, sanad.ConfirmDate, null, transfer.ToIssueTracking, ToSarfasl, ToTafsili, sanad.Description, sanad.TotalAmount, 0, false);
                        if (_blSanadItem.Save())
                        {
                            TempData["saveMassage"] = Notification.Show("انتقال وجه با موفقیت انجام شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                            return Json(new { RedirectUrl = Url.Action("Transfers", "Transaction") });
                        }
                        else
                        {
                            return Notification.jsonShow("انتقال وجه با خطا مواجه شد", type: ToastType.Error, position: ToastPosition.TopCenter);
                        }
                    }
                    else
                    {
                        return Notification.jsonShow("انتقال وجه با خطا مواجه شد", type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    return Notification.jsonShow("مبدا و مقصد انتقال وجه  مشخص نشده است", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                return Notification.jsonShow(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateTransaction)]
        [FiscalCheck]
        [SanadUpdateCheck]
        public ActionResult UpdateTransfer(int? TransferId)
        {
            if (TransferId.HasValue)
            {
                var model = new TransferViewModel();
                short CashDesk = (short)SarfaslEnums.CashDesk;
                var sanad = _blSanad.Find(TransferId.Value, Fiscal.GetFiscalId());
                model.SanadId = sanad.Id;
                var sanadItems = sanad.SanadItems;
                model.Amount = sanad.TotalAmount.ToPrice();
                model.Date = sanad.ConfirmDate.ToPersianDateTime().ToString("yyyy/MM/dd");
                model.Description = sanad.Description;
                var FirstItem = sanadItems.First();
                model.FirstItemId = FirstItem.Id;
                if (FirstItem.SarfaslId == CashDesk)
                {
                    model.IsFromCash = true;
                    short CashDeskId = FirstItem.Tafsili.CashDeskId.Value;
                    model.FromCashDesk = CashDeskId;
                }
                else
                {
                    model.IsFromCash = false;
                    short BankId = FirstItem.Tafsili.BankAccId.Value;
                    model.FromBank = BankId;
                    model.FromIssueTracking = FirstItem.IssueTrackingNum;
                }

                var LastItem = sanadItems.Last();
                model.SecondItemId = LastItem.Id;
                if (LastItem.SarfaslId == CashDesk)
                {
                    model.IsToCash = true;
                    short CashDeskId = LastItem.Tafsili.CashDeskId.Value;
                    model.ToCashDesk = CashDeskId;
                }
                else
                {
                    model.IsToCash = false;
                    short BankId = LastItem.Tafsili.BankAccId.Value;
                    model.ToBank = BankId;
                    model.ToIssueTracking = LastItem.IssueTrackingNum;
                }
                model.cashDesks = _blCashDesk.Select();
                model.Banks = _blBank.SelectView();

                model.Date = sanad.ConfirmDate.ToPersianDateTime().ToString("yyyy/MM/dd");
                return View(model);
            }
            else
            {
                return RedirectToAction("", "");
            }

        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.UpdateTransaction)]
        [FiscalCheck]

        public ActionResult UpdateTransfer(TransferViewModel transfer)
        {
            if (ModelState.IsValid)
            {
                if ((transfer.FromBank != 0 || transfer.FromCashDesk != 0) && (transfer.ToBank != 0 || transfer.ToCashDesk != 0))
                {
                    short FromId = transfer.FromBank == 0 ? transfer.FromCashDesk : transfer.FromBank;
                    short ToId = transfer.ToBank == 0 ? transfer.ToCashDesk : transfer.ToBank;

                    if (transfer.FromBank != 0 && transfer.ToBank != 0 && FromId == ToId)
                    {
                        return Notification.jsonShow("مبدا و مقصد انتقال باید متفاوت باشد", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    }

                    if (transfer.FromCashDesk != 0 && transfer.ToCashDesk != 0 && FromId == ToId)
                    {
                        return Notification.jsonShow("مبدا و مقصد انتقال باید متفاوت باشد", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    }
                    string FromName = "";
                    string ToName = "";
                    short FromSarfasl = 0;
                    int FromTafsili = 0;
                    short ToSarfasl = 0;
                    int ToTafsili = 0;
                    if (transfer.FromBank != 0)
                    {
                        FromName = _blBank.Find(FromId).Bank.Name;
                        var Tafsili = _blTafsili.Where(m => m.BankAccId == FromId && m.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                        FromSarfasl = (short)SarfaslEnums.Bank;
                        FromTafsili = Tafsili.Id;
                    }
                    if (transfer.FromCashDesk != 0)
                    {
                        FromName = _blCashDesk.Find(FromId).Name;
                        var Tafsili = _blTafsili.Where(m => m.CashDeskId == FromId && m.SarfaslId == (short)SarfaslEnums.CashDesk).FirstOrDefault();
                        FromSarfasl = (short)SarfaslEnums.CashDesk;
                        FromTafsili = Tafsili.Id;
                    }
                    if (transfer.ToBank != 0)
                    {
                        ToName = _blBank.Find(ToId).Bank.Name;
                        var Tafsili = _blTafsili.Where(m => m.BankAccId == ToId && m.SarfaslId == (short)SarfaslEnums.Bank).FirstOrDefault();
                        ToSarfasl = (short)SarfaslEnums.Bank;
                        ToTafsili = Tafsili.Id;
                    }
                    if (transfer.ToCashDesk != 0)
                    {
                        ToName = _blCashDesk.Find(ToId).Name;
                        var Tafsili = _blTafsili.Where(m => m.CashDeskId == ToId && m.SarfaslId == (short)SarfaslEnums.CashDesk).FirstOrDefault();
                        ToSarfasl = (short)SarfaslEnums.CashDesk;
                        ToTafsili = Tafsili.Id;
                    }
                    Sanad sanad = _blSanad.Find(transfer.SanadId, Fiscal.GetFiscalId());
                    sanad.ConfirmDate = transfer.Date.ToMiladiDate();
                    sanad.SanadTypeId = (byte)SandType.EntegalVajh;
                    sanad.IsAutomatic = true;
                    sanad.IsConfirmed = true;
                    sanad.CreatedByUserId = 1;
                    sanad.TotalAmount = transfer.Amount.ConvertToPrice();
                    if (string.IsNullOrEmpty(transfer.Description) || transfer.IsChanged == false)
                        sanad.Description = FromName.GetTransferDescription(ToName, transfer.FromIssueTracking, transfer.ToIssueTracking);
                    else
                        sanad.Description = transfer.Description;
                    short FiscalId = Fiscal.GetFiscalId();
                    sanad.PeriodId = FiscalId;
                    if (_blSanad.Update(sanad))
                    {
                        int id = sanad.Id;
                        _blSanadItem.Update(transfer.FirstItemId, id, FiscalId, sanad.ConfirmDate, null, transfer.FromIssueTracking, FromSarfasl, FromTafsili, sanad.Description, 0, sanad.TotalAmount, false);
                        _blSanadItem.Update(transfer.SecondItemId, id, FiscalId, sanad.ConfirmDate, null, transfer.ToIssueTracking, ToSarfasl, ToTafsili, sanad.Description, sanad.TotalAmount, 0, false);
                        if (_blSanadItem.Save())
                        {
                            TempData["saveMassage"] = Notification.Show("ویرایش وجه با موفقیت انجام شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                            return Json(new { RedirectUrl = Url.Action("Transfers", "Transaction", new { TransferId = sanad.Id }) });
                        }
                        else
                        {
                            return Notification.jsonShow("انتقال وجه با خطا مواجه شد", type: ToastType.Error, position: ToastPosition.TopCenter);
                        }
                    }
                    else
                    {
                        return Notification.jsonShow("انتقال وجه با خطا مواجه شد", type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    return Notification.jsonShow("مبدا و مقصد انتقال وجه  مشخص نشده است", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                return Notification.jsonShow(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

        }
        #endregion

        #region prop

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetAccountInfo(short BookId)
        {
            var bankAccount = _blBank.Find(BookId);
            return Json(new JsonData
            {
                Html = bankAccount.AccountNum,
                Option = bankAccount.Owner,
                Success = true
            });
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GenerateForm(int Row, int Formtype, bool ptaype = true)
        {
            var model = new PaymentViewModel();
            model.Row = Row;
            model.PaymentType = ptaype;
            model.Formtype = Formtype;
            string Form = "";
            switch (Formtype)
            {
                case 1:
                    model.CashDesks = _blCashDesk.Where(m => m.IsAvailable == true);
                    Form = this.RenderPartialToString("_FormGenerate", model);
                    break;
                case 2:
                    model.BankAccount = _blBank.SelectView();
                    Form = this.RenderPartialToString("_FormGenerate", model);
                    break;
                case 3:
                    model.ChequeBank = _blBankName.Select();
                    Form = this.RenderPartialToString("_FormGenerate", model);
                    break;
                case 4:
                    //  model.ChequeBankAccount = _blBank.SelectView(s => new SelectListItem { Value = s.BankAccountId.ToString(), Text = s.Name + "/" + s.AccountNum + "/" + s.Owner });
                    model.ChequeBankAccount = _blBank.SelectView();
                    Form = this.RenderPartialToString("_FormGenerate", model);
                    break;
                case 5:
                    model.PayCheque = _blcheque.Find(Row);
                    Form = this.RenderPartialToString("_FormGenerate", model);
                    break;
            };

            return Json(new JsonData
            {
                Html = Form
            });
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetPesronsInfo(int PersonType)
        {
            var model = new PaymentViewModel();
            model.PersonType = PersonType;
            string Form = "";
            switch (PersonType)
            {
                case 1:
                    model.Personnels = _blPersonnel.Select(s => new SelectListItem { Value = s.Id.ToString() + "/" + s.FirstName + " " + s.LastName, Text = s.FirstName + " " + s.LastName });
                    Form = this.RenderPartialToString("_DrpPerson", model);
                    break;
                case 2:
                    model.Customers = _blcustomers.Select(s => new SelectListItem { Value = s.Id.ToString() + "/" + s.OwnerName, Text = s.OwnerName });
                    Form = this.RenderPartialToString("_DrpPerson", model);
                    break;
                case 3:
                    model.Suppliers = _blsuppliers.Select(s => new SelectListItem { Value = s.Id.ToString() + "/" + s.Name, Text = s.Name });
                    Form = this.RenderPartialToString("_DrpPerson", model);
                    break;
            };

            return Json(new JsonData
            {
                Html = Form
            });
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult GetTafsiliInfo(short SarfaslId)
        {
            var model = new PaymentViewModel();
            model.Tafsili = _blTafsili.Where(m => m.SarfaslId == SarfaslId).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.TafsiliName });
            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_DrpTafsili", model)
            });
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetPersonBalance(int PersonId, int PersonType)
        {
            var model = new PaymentStatusViewModel();
            decimal bes = 0;
            decimal bed = 0;
            string Name = "";
            short FiscalId = Fiscal.GetFiscalId();
            if (PersonType == 1)
            {
                var PersonD = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                var PersonP = _blTafsili.Where(m => m.PersonnelId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                var bestan = _blSanadItem.Where(m => (m.TafsiliId == PersonD.Id || m.TafsiliId == PersonP.Id) && m.PeriodId == FiscalId);
                var bedeh = _blSanadItem.Where(m => (m.TafsiliId == PersonD.Id || m.TafsiliId == PersonP.Id) && m.PeriodId == FiscalId);
                if (bestan.Any())
                    bes = bestan.Sum(m => m.Bestan);
                else
                    bes = 0;

                if (bedeh.Any())
                    bed = bedeh.Sum(m => m.Bedeh);
                else
                    bes = 0;
                Name = PersonD.Personnel.FullName;
            }
            else if (PersonType == 2)
            {
                var PersonD = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                var PersonP = _blTafsili.Where(m => m.CustomerId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                var bestan = _blSanadItem.Where(m => (m.TafsiliId == PersonD.Id || m.TafsiliId == PersonP.Id) && m.PeriodId == FiscalId);
                var bedeh = _blSanadItem.Where(m => (m.TafsiliId == PersonD.Id || m.TafsiliId == PersonP.Id) && m.PeriodId == FiscalId);
                if (bestan.Any())
                    bes = bestan.Sum(m => m.Bestan);
                else
                    bes = 0;

                if (bedeh.Any())
                    bed = bedeh.Sum(m => m.Bedeh);
                else
                    bes = 0;
                Name = PersonD.Customer.OwnerName;
            }
            else
            {
                var PersonD = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabDaryaftani).FirstOrDefault();
                var PersonP = _blTafsili.Where(m => m.SupplierId == PersonId && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault();
                var bestan = _blSanadItem.Where(m => (m.TafsiliId == PersonD.Id || m.TafsiliId == PersonP.Id) && m.PeriodId == FiscalId);
                var bedeh = _blSanadItem.Where(m => (m.TafsiliId == PersonD.Id || m.TafsiliId == PersonP.Id) && m.PeriodId == FiscalId);
                if (bestan.Any())
                    bes = bestan.Sum(m => m.Bestan);
                else
                    bes = 0;

                if (bedeh.Any())
                    bed = bedeh.Sum(m => m.Bedeh);
                else
                    bes = 0;
                Name = PersonD.Supplier.Name;
            }
            model.Balance = bes - bed;
            model.Name = Name;
            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_HesabStatus", model)
            });
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetAvailableCheque()
        {
            var model = new PaymentViewModel();
            model.AvailableCheque = _blcheque.Where(m => m.StatusId == 1);
            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_AvailableChequeList", model)
            });
        }
        #endregion
    }
}