using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class InventoryCheckingMetaData
    {        
        [Display(Name = "شناسه")]
        public short Id { get; set; }

        [Display(Name = "انبار")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short WarehouseId { get; set; }

        [Display(Name = "شرح")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }

        [Display(Name = "تاریخ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public int CreatedByUserId { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(InventoryCheckingMetaData))]
    public partial class InventoryChecking
    {

    }
}