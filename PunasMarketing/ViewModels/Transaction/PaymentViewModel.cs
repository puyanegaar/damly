using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PunasMarketing.ViewModels.Transaction
{
    public class PaymentViewModel
    {
        //CashPart
        public PunasMarketing.Models.LocalModel.CashModel[] cash { get; set; }

        public PunasMarketing.Models.LocalModel.CashModel[] EditCash { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.CashDesk> CashDesks { get; set; }



        //CardPart
        public PunasMarketing.Models.LocalModel.CardModel[] Card { get; set; }
        public PunasMarketing.Models.LocalModel.CardModel[] EditCard { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.BankAccountView> BankAccount { get; set; }

        //Cheque
        public PunasMarketing.Models.DomainModel.Cheque[] cheques { get; set; }

        public PunasMarketing.Models.DomainModel.Cheque[] Editcheques { get; set; }

        public PunasMarketing.Models.DomainModel.Cheque[] EditPaycheques { get; set; }

        public IEnumerable<PunasMarketing.Models.DomainModel.BankAccountView> ChequeBankAccount { get; set; }
     
        public IEnumerable<PunasMarketing.Models.DomainModel.Bank> ChequeBank { get; set; }

      
        //Receive
        public PunasMarketing.Models.LocalModel.PaymentModel pay { get; set; }


        //personsList 
        public IEnumerable<SelectListItem> Personnels { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; }

        public IEnumerable<SelectListItem> Suppliers { get; set; }

        public int PersonType { get; set; }

        //Sarfasl
        public IEnumerable<SelectListItem> Sarfasls { get; set; }

        public IEnumerable<SelectListItem> Tafsili { get; set; }

        //AvailableCheque
        public IEnumerable<PunasMarketing.Models.DomainModel.Cheque> AvailableCheque { get; set; }

        public string PayChequeIds { get; set; }

        public PunasMarketing.Models.DomainModel.Cheque PayCheque { get; set; }
        public string NowDate { get; set; }

        public string deletedItem {get; set;}

        public string deteledCheque { get; set; }

        public string deteledPayCheque { get; set; }
        public int Row { get; set; }
        public int Formtype { get; set; }

        public bool PaymentType { get; set; }

        public decimal Taraz { get; set; }
    }
}