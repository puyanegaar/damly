using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class CustomerCategoryResponse
    {
        public short Id { get; set; }

        [Display(Name = "عنوان دسته بندی مشتری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }
    }
}