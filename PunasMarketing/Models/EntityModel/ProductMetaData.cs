using System;
using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class ProductMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "کد کالا")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string ProductCode { get; set; }

        [Display(Name = "نام کالا")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "واحد اصلی (واحد کوچک)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short MainUnitId { get; set; }

        [Display(Name = "واحد فرعی (واحد بزرگ)")]
        public short? SubUnitId { get; set; }

        [Display(Name = "نسبت واحد فرعی به اصلی")]
        public double? UnitRate { get; set; }

        [Display(Name = "انبار")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short WarehouseId { get; set; }

        [Display(Name = "دسته بندی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public int ProductCategoryId { get; set; }

        [Display(Name = "موجودی")]
        public int Inventory { get; set; }

        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

        [Display(Name = "کاتالوگ")]
        public string CatalogFileName { get; set; }

        [Display(Name = "در دسترس")]
        public bool IsAvailable { get; set; }

        [Display(Name = "قابل فروش")]
        public bool IsSellable { get; set; }

        [Display(Name = "سهم بازاریاب (ریال)")]
        public decimal? MarketerCommission { get; set; }

        [Display(Name = "نقطه سفارش")]
        public int? OrderPoint { get; set; }

        [Display(Name = "در انتظار حواله")]
        public double PendingsCount { get; set; }

        [Display(Name = "وضعیت تولید")]
        public byte? ProductionStatus { get; set; }

        [Display(Name = "توضیحات")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {

    }
}