using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.LocalModel
{
    public class PaymentModel
    {
        [Display(Name = "شرح")]
        public string Description { get; set; }

        [Display(Name = "نوع شخص")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public int PersonType { get; set; }

        [Display(Name = "حساب شخص")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "{0} را انتخاب نمایید")]
        public string Persons { get; set; } = "-1";

        [Display(Name = "حساب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public int sarfasl { get; set; }

        public string sarfaslText { get; set; }

        [Display(Name = "تفضیلی")]
        public int? Tafsili { get; set; }

        public string TafsiliText { get; set; }

        public Nullable<decimal> TotalAmount { get; set; }

        public int SanadId { get; set; }

        public short FiscalId { get; set; }
    }
}