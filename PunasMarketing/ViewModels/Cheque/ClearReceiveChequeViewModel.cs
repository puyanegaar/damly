using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.ViewModels.Cheque
{
    public class ClearReceiveChequeViewModel
    {
        public int ChequeId { get; set; }

        [Display(Name = "شماره حساب واریزی")]
        public short? BankAccountId { get; set; }

        [Display(Name = "صندوق")]
        public short? CashDeskId { get; set; }

        [Display(Name = "تاریخ وصول چک")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string ClearDate { get; set; }

        public IEnumerable<Models.DomainModel.BankAccount> BankAccounts { get; set; }
        public IEnumerable<Models.DomainModel.CashDesk> CashDesks { get; set; }
    }
}