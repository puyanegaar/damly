using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.LocalModel
{
    public class SanadModel
    {
        public int SanadId { get; set; }

        [Display(Name = "شرح سند")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string Description { get; set; }

        [Display(Name = "تاریخ سند")]
        public string SanadDate { get; set; }
    }
}