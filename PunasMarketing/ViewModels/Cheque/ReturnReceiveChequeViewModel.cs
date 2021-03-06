﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Cheque
{
    public class ReturnReceiveChequeViewModel
    {
        public int ChequeId { get; set; }

        [Display(Name = "تاریخ عودت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string ReturnDate { get; set; }
    }
}