using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class OrderItemMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "شماره سفارش")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        public int OrderId { get; set; }

        [Display(Name = "نام کالا")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        public short ProductId { get; set; }

        [Display(Name = "تعداد واحد اصلی")]
        public double? MainUnitCount { get; set; }

        [Display(Name = "تعداد واحد فرعی")]
        public double? SubUnitCount { get; set; }

        [Display(Name = "نسبت واحدها")]
        public double? UnitRate { get; set; }

        [Display(Name = "قیمت واحد")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "تخفیف واحد")]
        public decimal UnitDiscount { get; set; }

        [Display(Name = "توضیحات")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(OrderItemMetaData))]
    public partial class OrderItem
    {

    }
}