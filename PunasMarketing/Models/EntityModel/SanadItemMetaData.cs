using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class SanadItemMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        public short PeriodId { get; set; }

        [Display(Name = "شماره سند")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public int SandId { get; set; }

        public int? ChequeId { get; set; }

        [Required]
        public short SarfaslId { get; set; }

        [Display(Name = "شرح")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string Description { get; set; }

        [Display(Name = "بدهکار")]
        public decimal Bedeh { get; set; }

        [Display(Name = "بستانکار")]
        public decimal Bestan { get; set; }

        public Nullable<System.DateTime> ConfirmDate { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(SanadItemMetaData))]
    public partial class SanadItem
    {
       
    }
}