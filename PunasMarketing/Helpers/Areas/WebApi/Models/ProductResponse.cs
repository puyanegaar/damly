using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class ProductResponse
    {
        public int Id { get; set; }

        [Display(Name = "کد کالا")]
        public string ProductCode { get; set; }

        [Display(Name = "نام کالا")]
        public string Name { get; set; }

        [Display(Name = "واحد اصلی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short MainUnitId { get; set; }

        [Display(Name = "دسته بندی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public int ProductCategoryId { get; set; }

        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

        [Display(Name = "وضعیت تولید")]
        public string ProductionStatus { get; set; }

        [Display(Name = "توضیحات")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}