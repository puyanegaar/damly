using PunasMarketing.Models.EntityModel;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{
    internal class SupplierMetaData
    {
        [Display(Name = "شناسه")]
        public short Id { get; set; }

        [Display(Name = "نام تأمین کننده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(70, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "موبایل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(15, ErrorMessage = "تعداد ارقام نمی تواند بیش از {1} باشد")]
        public string Mobile { get; set; }

        [Display(Name = "تلفن ثابت")]
        [StringLength(15, ErrorMessage = "تعداد ارقام نمی تواند بیش از {1} باشد")]
        public string Phone { get; set; }

        [Display(Name = "شهر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short CityId { get; set; }

        [Display(Name = "آدرس")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "توضیحات")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

}

namespace PunasMarketing.Models.DomainModel
{

    [MetadataType(typeof(SupplierMetaData))]
    public partial class Supplier
    {
        [Display(Name = "استان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public int ProvinceId { get; set; }
    }
}