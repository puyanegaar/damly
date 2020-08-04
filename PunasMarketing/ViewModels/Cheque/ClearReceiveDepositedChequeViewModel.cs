using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Cheque
{
    public class ClearReceiveDepositedChequeViewModel
    {
        public int ChequeId { get; set; }

        [Display(Name = "تاریخ وصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string DepositDate { get; set; }
    }
}