using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class FactorMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "شماره فاکتور مرجوعی")]
        public int? ReturnForFactorId { get; set; }

        [Display(Name = "شماره فاکتور کاغذی")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string PaperFactorCode { get; set; }

        [Display(Name = "نام تامین کننده")]
        public short? SupplierId { get; set; }

        [Display(Name = "نام مشتری")]
        public int? CustomerId { get; set; }

        [Display(Name = "قیمت کل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "تخفیف روی فاکتور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal DiscountOnFactor { get; set; }

        [Display(Name = "تخفیف کل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal TotalDiscount { get; set; }

        [Display(Name = "اضافات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal Additions { get; set; }

        [Display(Name = "کسورات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal Deductions { get; set; }

        [Display(Name = "مالیات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal TotalTax { get; set; }

        [Display(Name = "قیمت نهایی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal FinalPrice { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public int CreatorUserId { get; set; }

        [Display(Name = "تاریخ فاکتور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "بازاریاب")]
        public int? MarketerId { get; set; }

        [Display(Name = "پورسانت بازاریاب")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public decimal MarketerTotalCommission { get; set; }

        [Display(Name = "آیا فاکتور خرید است؟")]
        public bool IsBuy { get; set; }

        [Display(Name = "آیا پیش فاکتور است؟")]
        public bool IsPreFactor { get; set; }

        [Display(Name = "فاکتور تأیید شده")]
        public bool? IsConfirmed { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }


    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(FactorMetaData))]
    public partial class Factor
    {
        public string WareHouse { get; set; }
    }
}
