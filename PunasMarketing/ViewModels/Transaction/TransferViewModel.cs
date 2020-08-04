using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Transaction
{
    public class TransferViewModel
    {
        public int SanadId { get; set;}

        public int FirstItemId { get; set; }

        public int SecondItemId { get; set; }

        [Display(Name = "مبلغ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string Amount { get; set; }

        [Display(Name = "تاریخ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string Date { get; set; }

        [Display(Name = "شرح انتقال")]
        public string Description { get; set; }

        [Display(Name = "صندوق")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short FromCashDesk { get; set; }

        [Display(Name = "صندوق")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short ToCashDesk { get; set; }

        [Display(Name = "بانک")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short FromBank { get; set; }

        [Display(Name = "بانک")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short ToBank { get; set; }

        [Display(Name = "شماره ارجاع")]
        public string FromIssueTracking { get; set; }

        [Display(Name = "شماره ارجاع")]
        public string ToIssueTracking { get; set; }

        public IEnumerable<PunasMarketing.Models.DomainModel.BankAccountView> Banks { get; set; }

        public IEnumerable<PunasMarketing.Models.DomainModel.CashDesk> cashDesks { get; set; }

        public bool IsFromCash { get; set; }

        public bool IsToCash { get; set; }

        public bool IsChanged {get; set;}
    }
}