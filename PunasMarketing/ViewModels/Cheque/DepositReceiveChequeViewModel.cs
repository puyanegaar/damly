using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.ViewModels.Cheque
{
    public class DepositReceiveChequeViewModel
    {
        public int ChequeId { get; set; }

        [Display(Name = "شماره حساب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short BankAccountId { get; set; }

        public IEnumerable<Models.DomainModel.BankAccount> BankAccounts { get; set; }

        [Display(Name = "تاریخ واگذاری به بانک")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string DepositDate { get; set; }
    }
}