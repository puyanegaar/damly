using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class OfferMetaData
    {
        [Display(Name = "شناسه")]
        public short Id { get; set; }

        [Display(Name = "عنوان جشنواره")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }

        //[Display(Name = "تاریخ آغاز جشنواره")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public DateTime StartDate { get; set; }

        //[Display(Name = "تاریخ پایان جشنواره")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public DateTime ExpDate { get; set; }

        [Display(Name = "تصویر جشنواره")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public string ImageName { get; set; }

        [Display(Name = "دسته مشتریانی که جشنواره برای آن ها قابل نمایش و قابل اعمال روی فاکتور است")]
        public string ForCustomerCategories { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "نمایش در اسلایدر")]
        public bool ShowInSlider { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(OfferMetaData))]
    public partial class Offer
    {
       
    }
}