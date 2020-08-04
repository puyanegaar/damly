using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Sarfasl
{
    public class SarfaslViewModel
    {
        public Models.DomainModel.Sarfasl Sarfasl { get; set; }

        public IEnumerable<Models.DomainModel.Sarfasl> AllSarfasls { get; set; }

        [Display(Name = "حساب کل")]
        public IEnumerable<Models.DomainModel.Sarfasl> KolSarfasls { get; set; }

        [Display(Name = "گروه حساب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب کنید")]
        public short GroupSarfaslId { get; set; }

        [Display(Name = "حساب کل")]
        public short? KolSarfaslId { get; set; }
    }
}