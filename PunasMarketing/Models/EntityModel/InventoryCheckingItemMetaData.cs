using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class InventoryCheckingItemMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        public short InventoryCheckingId { get; set; }

        [Display(Name = "کالا")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short ProductId { get; set; }

        [Display(Name = "موجودی واقعی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public double RealMainUnitCount { get; set; }

        [Display(Name = "موجودی سیستم")]
        public double SystemMainUnitCount { get; set; }

        [Display(Name = "اختلاف موجودی")]
        public double Difference { get; set; }

        [Display(Name = "تعدیل شده")]
        public bool IsCorrected { get; set; }

        [Display(Name = "شرح")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(InventoryCheckingItemMetaData))]
    public partial class InventoryCheckingItem
    {

    }
}