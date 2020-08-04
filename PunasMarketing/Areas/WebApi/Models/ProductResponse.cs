using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class ProductResponse
    {
        [Display(Name = "شناسه")]
        public short Id { get; set; }

        [Display(Name = "کد کالا")]
        public string ProductCode { get; set; }

        [Display(Name = "نام کالا")]
        public string Name { get; set; }

        [Display(Name = "شناسه واحد اصلی")]
        public short MainUnitId { get; set; }

        [Display(Name = "نام واحد اصلی")]
        public string MainUnitName { get; set; }

        [Display(Name = "شناسه دسته بندی")]
        public int ProductCategoryId { get; set; }

        [Display(Name = "نام دسته بندی")]
        public string ProductCategoryName { get; set; }

        [Display(Name = "قیمت مشتری")]
        public long CustomerPrice { get; set; }

        [Display(Name = "تخفیفات")]
        public DiscountResponse Discount { get; set; }

        [Display(Name = "کاتالوگ")]
        public string CatalogFileName { get; set; }

        [Display(Name = "تصاویر")]
        public List<string> Images { get; set; }

        [Display(Name = "وضعیت تولید")]
        public string ProductionStatus { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }

    public class DiscountResponse
    {
        [Display(Name = "قیمت همراه با تخفیف")]
        public long? PriceWithDiscount { get; set; }

        [Display(Name = "شناسه کالای هدیه")]
        public short? GiftProductId { get; set; }

        [Display(Name = "نام کالای هدیه")]
        public string GiftProductName { get; set; }

        [Display(Name = "مقدار کالای هدیه")]
        public double? GiftProductCount { get; set; }

        [Display(Name = "حداقل تعداد کالا")]
        public double? MinProductCount { get; set; }
    }
}