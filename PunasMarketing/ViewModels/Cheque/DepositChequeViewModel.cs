using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.ViewModels.Cheque
{
    public class DepositChequeViewModel
    {
        public int ChequeId { get; set; }

        [Display(Name = "شماره حساب واریزی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short BankAccountId { get; set; }

        public IEnumerable<Models.DomainModel.BankAccount> BankAccounts { get; set; }
    }
}