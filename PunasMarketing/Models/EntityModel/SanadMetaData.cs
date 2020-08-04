using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class SanadMetaData
    {
        [Display(Name = "شناسه سند")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public int Id { get; set; }

        public short PeriodId { get; set; }

        [Display(Name = "شرح")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(300, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "نوع")]
        public bool IsAutomatic { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public int CreatedByUserId { get; set; }

        [Display(Name = "تاریخ ثبت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "تاریخ تأیید")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public DateTime ConfirmDate { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(SanadMetaData))]
    public partial class Sanad
    {
    }
}