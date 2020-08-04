using System.Collections.Generic;

namespace PunasMarketing.ViewModels.Receipt
{
    public class ReceiptViewModel
    {
        public PunasMarketing.Models.DomainModel.Receipt receipt { get; set; }
        public PunasMarketing.Models.LocalModel.CashModel cash { get; set; }
        public PunasMarketing.Models.LocalModel.PoseModel Pose { get; set; }
        public PunasMarketing.Models.LocalModel.CardModel Card { get; set; }

        public PunasMarketing.Models.DomainModel.Cheque cheque { get; set; }

        public PunasMarketing.Models.DomainModel.Cheque ourcheque { get; set; }

        public IEnumerable<PunasMarketing.Models.DomainModel.Cheque> chequeList { get; set; }

        public IEnumerable<PunasMarketing.Models.DomainModel.Cheque> ourchequeList { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.BankAccount> Banks { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.Cost> costs { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.Customer> customers { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.Supplier> suppliers { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.Personnel> personnels { get; set; }

        public IEnumerable<PunasMarketing.Models.DomainModel.CashDesk> CashDesks { get; set; }

        public IEnumerable<PunasMarketing.Models.DomainModel.Bank> BankName { get; set; }

        public IEnumerable<PunasMarketing.Models.LocalModel.BankAccountDisplayModel> BankAccount { get; set; }
        public IEnumerable<PunasMarketing.Models.LocalModel.BankAccountDisplayModel> BankAccountPose { get; set; }
        public byte type { get; set; }
        public string NowDate { get; set; }
        public bool IsCash { get; set; }

        public bool IsPose { get; set; }

        public bool IsCard { get; set; }
        public bool IsOthersCheque { get; set; }
        public bool IsOurCheque { get; set; }
        public string ChequeIdArray { get; set; }
        public string OurChequeIdArray { get; set; }
        public bool ChequeType { get; set; }

        public string CounterPartyText { get; set; }
    }
}

