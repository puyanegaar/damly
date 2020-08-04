using PunasMarketing.Models.EntityModel;
using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.EntityModel
{
    public class FactorItemMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "شماره فاکتور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public int FactorId { get; set; }

        [Display(Name = "نام کالا")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public short ProductId { get; set; }

        [Display(Name = "تعداد واحد اصلی")]
        public double? MainUnitCount { get; set; }

        [Display(Name = "تعداد واحد فرعی")]
        public double? SubUnitCount { get; set; }

        [Display(Name = "نسبت واحدها")]
        public double? UnitsRate { get; set; }

        [Display(Name = "قیمت واحد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "واحد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public int UnitId { get; set; }

        [Display(Name = "تخفیف کالا")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal Discount { get; set; }

        [Display(Name = "مالیات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal Tax { get; set; }

        [Display(Name = "پورسانت بازاریاب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal MarketerCommission { get; set; }

        [Display(Name = "قیمت کل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "قیمت نهایی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal FinalPrice { get; set; }

        [Display(Name = "توضیحات")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }

    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(FactorItemMetaData))]
    public partial class FactorItem
    {

        [Display(Name = "نوع قیمت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public short PriceTypeId { get; set; }
    }
}
