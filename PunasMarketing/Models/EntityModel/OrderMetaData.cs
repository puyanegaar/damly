using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class OrderMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "مشتری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        public int CustomerId { get; set; }

        [Display(Name = "بازاریاب")]
        public int? MarketerId { get; set; }

        [Display(Name = "تاریخ ثبت سفارش")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "وضعیت تایید")]
        public bool IsVerified { get; set; }

        [Display(Name = "شماره فاکتور")]
        public int? FactorId { get; set; }

        [Display(Name = "دوره مالی")]
        public short? PeriodId { get; set; }

        [Display(Name = "جمع مبلغ سفارش")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "شرح")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(OrderMetaData))]
    public partial class Order
    {
      
    }
}